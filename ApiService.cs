using System.Net.Http.Headers;
using System.Text.Json;

public class ApiService: iApiService {
  HttpClient client = new HttpClient();
  string apiBaseURL = "http://scheduling-interview-2021-265534043.us-west-2.elb.amazonaws.com/";
  string token = "?token=d0ad84d9-f051-46a7-a8da-35d131dc9bf4";

  public ApiService(string? token) {
    this.token = "?token=" + token; 
    client.BaseAddress = new Uri(apiBaseURL);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
  }
  
  public List<AppointmentInfo> GetSchedule() {
    List<AppointmentInfo> appts = new List<AppointmentInfo>();
    
    try {
      HttpResponseMessage response = client.GetAsync("api/Scheduling/Schedule" + token).Result;
        if (response.IsSuccessStatusCode) {
          Console.WriteLine("Schedule Success!");
          string jsonStr = response.Content.ReadAsStringAsync().Result;
          var deserialized = JsonSerializer.Deserialize<List<AppointmentInfo>>(jsonStr);
          if (deserialized != null) {
            appts = deserialized;
          }
        }
        else {
          Console.WriteLine("Schedule Failure!");
          Console.WriteLine(response.ReasonPhrase);
        }
    }
    catch (Exception ex) {
      Console.WriteLine(ex.Message);
    }

    return appts;
  }
  public void Start() {
    try {
      HttpResponseMessage response = client.PostAsync("api/Scheduling/Start" + token, null).Result;
        if (response.IsSuccessStatusCode) {
          Console.WriteLine("Start Success!");
        }
        else {
          Console.WriteLine("Start Failure!");
        }
    }
    catch (Exception ex) {
      Console.WriteLine(ex.Message);
    }
  }
  public void Stop() {
    try {
      HttpResponseMessage response = client.PostAsync("api/Scheduling/Stop" + token, null).Result;
        if (response.IsSuccessStatusCode) {
          Console.WriteLine("Stop Success!");
          Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
        else {
          Console.WriteLine("Stop Failure!");
        }
    }
    catch (Exception ex) {
      Console.WriteLine(ex.Message);
    }
  }
  public AppointmentRequest? GetAppointmentRequest() {
    AppointmentRequest? apptRequest = new AppointmentRequest();

    try {
      HttpResponseMessage response = client.GetAsync("api/Scheduling/AppointmentRequest" + token).Result;
        if (response.IsSuccessStatusCode) {
          if (((int)response.StatusCode) == 204) {
            apptRequest = null;
          }
          else {
            Console.WriteLine("Request Success!");
            string jsonStr = response.Content.ReadAsStringAsync().Result;
            var deserialized = JsonSerializer.Deserialize<AppointmentRequest>(jsonStr);
            if (deserialized != null) {
              apptRequest = deserialized;
            }
          }
        }
        else {
          Console.WriteLine("Request Failure!");
          Console.WriteLine(response.ReasonPhrase);
        }
    }
    catch (Exception ex) {
      Console.WriteLine(ex.Message);
    }

    return apptRequest;
  }
  public void Schedule(AppointmentInfoRequest appt) {
    try {
      string jsonStr = JsonSerializer.Serialize(appt);
      StringContent payload = new StringContent(jsonStr, System.Text.Encoding.UTF8, "application/json");
      HttpResponseMessage response = client.PostAsync("api/Scheduling/Schedule" + token, payload).Result;
        if (response.IsSuccessStatusCode) {
          Console.WriteLine("Schedule Success!");
        }
        else {
          Console.WriteLine("Schedule Failure!");
          Console.WriteLine(response.StatusCode + ": " + response.ReasonPhrase);
        }
    }
    catch (Exception ex) {
      Console.WriteLine(ex.Message);
    }
  }
}