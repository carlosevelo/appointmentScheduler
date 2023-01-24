using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

class Scheduler {
  public iApiService apiService;
  public List<AppointmentInfo> scheduledAppts;
  public Dictionary<Doctor, List<AppointmentInfo>> doctorSchedule;
  public Dictionary<int, List<AppointmentInfo>> personSchedule;

  public Scheduler(ApiService apiService) {
    this.apiService = apiService;
    apiService.Start();
    scheduledAppts = apiService.GetSchedule();
    doctorSchedule = new Dictionary<Doctor, List<AppointmentInfo>>();
    personSchedule = new Dictionary<int, List<AppointmentInfo>>();
    calculateSchedules(scheduledAppts);
  }

  public void calculateSchedules(List<AppointmentInfo> scheduledAppts) {
    foreach (AppointmentInfo appt in scheduledAppts) {
      if (appt.appointmentTime < DateTime.Now) {
        continue;
      }

      //Doctor Schedules
      if (doctorSchedule.ContainsKey(appt.doctorId)) {
        doctorSchedule[appt.doctorId].Add(appt);
      }
      else {
        doctorSchedule.Add(appt.doctorId, new List<AppointmentInfo>());
        doctorSchedule[appt.doctorId].Add(appt);
      }
      //Person Schedules
      if (personSchedule.ContainsKey(appt.personId)) {
        personSchedule[appt.personId].Add(appt);
      }
      else {
        personSchedule.Add(appt.personId, new List<AppointmentInfo>());
        personSchedule[appt.personId].Add(appt);
      }
    }
  }

  public AppointmentRequest? GetNewRequest() {
    return apiService.GetAppointmentRequest();
  }

  public void ScheduleAppointment(AppointmentRequest request) {
    List<AppointmentInfo> personAppts = new List<AppointmentInfo>();
    if (personSchedule.ContainsKey(request.personId)) {
      personAppts = personSchedule[request.personId].OrderBy(a => a.appointmentTime).ToList();
    }
    
    //DAY FILTERING
    DateTime possibleDateTime = new DateTime(2021, 11, 1, 8, 0, 0);
    
    //Appointments may only be scheduled on weekdays during the months of November and December 2021.
    if (6 < ((int)possibleDateTime.DayOfWeek) && ((int)possibleDateTime.DayOfWeek) < 2) {
      int daysToAdd = (2 - (int) possibleDateTime.DayOfWeek + 7) % 7;
      possibleDateTime.AddDays(daysToAdd);
    }
    //For a given patient, each appointment must be separated by at least one week.
    DateTime tempDate = possibleDateTime;
    foreach (AppointmentInfo apptInfo in personAppts) {
      if (apptInfo.appointmentTime < tempDate.AddDays(7)) {
        possibleDateTime = apptInfo.appointmentTime.AddDays(7);
        if (6 < ((int)possibleDateTime.DayOfWeek) && ((int)possibleDateTime.DayOfWeek) < 2) {
          int daysToAdd = (2 - (int) possibleDateTime.DayOfWeek + 7) % 7;
          possibleDateTime.AddDays(daysToAdd);
        }
        tempDate = possibleDateTime;
      }
      else {
        //TODO: Doctor/Time filtering
        //Appointments can be scheduled as early as 8 am UTC and as late as 4 pm UTC.
        //For a given doctor, you may only have one appointment scheduled per hour 

        break;
      }
    }
  
    //Appointments for new patients may only be scheduled for 3 pm and 4 pm.
    if (request.isNew) {
      possibleDateTime.AddHours(7);
    }

    //TODO: Pass in correct doctor
    doctorSchedule[Doctor.doc1].Add(new AppointmentInfo(Doctor.doc1, request.personId, possibleDateTime, request.isNew));
    personSchedule[request.personId].Add(new AppointmentInfo(Doctor.doc1, request.personId, possibleDateTime, request.isNew));
    
    AppointmentInfoRequest appt = new AppointmentInfoRequest(request, Doctor.doc1, possibleDateTime);
    apiService.Schedule(appt);
  }
}