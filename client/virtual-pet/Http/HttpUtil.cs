using System;
using System.Text;
using System.Text.Json;
using VirtualPetSchool.Model;

namespace VirtualPetSchool.Http {
    internal class HttpUtil {
        private const string API_URL = "http://localhost:8080/virtualpet";

        async public static Task<HttpResponse> BreedPet(VirtualPet pet) {
            try {
                using (HttpClient client = new HttpClient()) {
                    HttpContent content = new StringContent(JsonSerializer.Serialize(pet), Encoding.UTF8, "application/json");
                    using (HttpResponseMessage response = await client.PostAsync(API_URL, content)) {
                        object body = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                            body = JsonSerializer.Deserialize<VirtualPet>((string)body);
                        }
                        return new HttpResponse(response.StatusCode, body);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine($"Error while creating pet: {e.Message}");
                return new HttpResponse(System.Net.HttpStatusCode.InternalServerError, "Check logs");
            }
        }

        async public static Task<HttpResponse> LoadPet(string name) {
            try {
                using (HttpClient client = new HttpClient()) {
                    using (HttpResponseMessage response = await client.GetAsync(API_URL + "?name=" + name)) {
                        object body = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                            body = JsonSerializer.Deserialize<VirtualPet>((string)body);
                        }
                        return new HttpResponse(response.StatusCode, body);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine($"Error while loading pet: {e.Message}");
                return new HttpResponse(System.Net.HttpStatusCode.InternalServerError, "Check logs");
            }
        }

        async public static Task<HttpResponse> ListPets(bool alive) {
            try {
                string route = "/list" + (alive
                    ? "?alive=true"
                    : "");
                using (HttpClient client = new HttpClient()) {
                    using (HttpResponseMessage response = await client.GetAsync(API_URL + route)) {
                        object body = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                            body = JsonSerializer.Deserialize<VirtualPet[]>((string)body);
                        }
                        return new HttpResponse(response.StatusCode, body);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine($"Error while loading pet: {e.Message}");
                return new HttpResponse(System.Net.HttpStatusCode.InternalServerError, "Check logs");
            }
        }

        async public static Task<HttpResponse> DeletePet(string name) {
            try {
                using (HttpClient client = new HttpClient()) {
                    using (HttpResponseMessage response = await client.DeleteAsync(API_URL + "?name=" + name)) {
                        object body = await response.Content.ReadAsStringAsync();
                        return new HttpResponse(response.StatusCode, body);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine($"Error while deleting pet: {e.Message}");
                return new HttpResponse(System.Net.HttpStatusCode.InternalServerError, "Check logs");
            }
        }

        async public static Task<HttpResponse> UpdatePet(VirtualPet pet) { 
            try {
                using (HttpClient client = new HttpClient()) {
                    HttpContent content = new StringContent(JsonSerializer.Serialize(pet), Encoding.UTF8, "application/json");
                    using (HttpResponseMessage response = await client.PutAsync(API_URL, content)) {
                        object body = await response.Content.ReadAsStringAsync();
                        return new HttpResponse(response.StatusCode, body);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine($"Error while updating pet: {e.Message}");
                return new HttpResponse(System.Net.HttpStatusCode.InternalServerError, "Check logs");
            }
        }
    }
}
