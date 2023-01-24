namespace AppoinmentScheduler
{
  class AppoinmentScheduler
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Starting Scheduler...");
      Console.WriteLine("Initializing Schedule...");
      Scheduler scheduler = new Scheduler(new ApiService());
      
      AppointmentRequest? request = scheduler.GetNewRequest();
      if (request != null) {
        scheduler.ScheduleAppointment(request);
      }
      // while(request != null) {
      //   scheduler.ScheduleAppointment(request);
      //   request = scheduler.GetNewRequest();
      // }
    }
  }
}