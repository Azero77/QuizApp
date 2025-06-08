using Microsoft.AspNetCore.Components;
using System.Net;

namespace QuizApp.BlazorWASM.Services.HttpClientHandlers
{
    public class HttpClientErrorDelegationHandler : DelegatingHandler
    {
        private readonly NavigationManager _navigationManager;

        public HttpClientErrorDelegationHandler(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response;
            try
            {

                response = await base.SendAsync(request, cancellationToken);
            }
            catch (HttpRequestException e)
            {
                OnErrorsChanged("Request Failed, Try another time");
                return new HttpResponseMessage(e.StatusCode ?? HttpStatusCode.BadRequest);
            }
            catch (TaskCanceledException)
            {
                OnErrorsChanged("Connection is slow, try another time");
                return new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            }
            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        // Handle 404 errors
                        OnErrorsChanged("Resource not found.");

                        break;
                    case HttpStatusCode.Unauthorized:
                        // Handle 401 errors
                        OnErrorsChanged("Unauthorized access.");
                        break;
                    case HttpStatusCode.InternalServerError:
                        // Handle 500 errors
                        OnErrorsChanged("Server error.");
                        break;
                    default:
                        // Handle other errors
                        OnErrorsChanged($"An error occurred: {response.StatusCode}");
                        break;
                }
            }
            return response;
        }

        public event Action<string>? ErrorsChanged = null;

        private void OnErrorsChanged(string error_message)
        {
            ErrorsChanged?.Invoke(error_message);
        }
    }
}
