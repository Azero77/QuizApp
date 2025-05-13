using DocumentFormat.OpenXml.InkML;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using QuizApp.Models;
using QuizApp.Shared;
using System.Security.Claims;
using System.Text.Json;

namespace QuizApp.API.Authorization
{
    /// <summary>
    /// Users are allowed to see their submission only and no other submission
    /// </summary>
    public class OnlyOwnedSubmissionFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.HttpContext.User.IsInRole(ApplicationConstants.Roles.Admin))
                return;
            if (context.HttpContext.Response.StatusCode == StatusCodes.Status200OK)
            {
                string user_id = context.HttpContext.User?.Identity?.GetSubjectId() ?? string.Empty;
                string? submission_user_id = GetUserIdFromSubmission(context.Result);
                if (user_id != submission_user_id)
                    context.HttpContext.Response.StatusCode = 403;
            }
        }       

        private string? GetUserIdFromSubmission(IActionResult result)
        {
            if (result is ObjectResult objectResult)
            {
                string json = JsonConvert.SerializeObject(objectResult.Value);
                Console.WriteLine(json);
                Submission? sub = JsonConvert.DeserializeObject<Submission>(json);
                if (sub is not null)
                    return sub.SubmissionPersonName;
            }
            return null;
        }
    }
}
