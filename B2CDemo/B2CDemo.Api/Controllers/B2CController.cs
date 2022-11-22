using Microsoft.AspNetCore.Mvc;

namespace B2CDemo.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class B2CController : ControllerBase
    {
        public class UserModel
        {
            public int MemberNumber { get; set; }
        }

        public class ValidateWDNIResponse
        {
            public bool IsValidUser { get; set; }
        }

        public class TermsOfUseModelRequest
        {
            public int MemberNumber { get; set; }
            public string Email { get; set; }
        }

        public class SaveUserRequest
        {
            public int MemberNumber { get; set; }
        }

        public class SaveUserResponse
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Username { get; set; }
        }

        public class RemoteLoginResponse
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string DisplayName { get; set; }
        }

        public class SamelessRemoteLoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class SamelessRemoteLoginResponse
        {
            public bool tokenSuccess { get; set; }
            public bool migrationRequired { get; set; }
        }

        private readonly ILogger<B2CController> _logger;

        public B2CController(ILogger<B2CController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("validate-member-number")]
        [ProducesResponseType(typeof(ValidateWDNIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public ValidateWDNIResponse ValidateMemberNumber(UserModel user)
        {
            _logger.LogInformation($"Called ValidateMemberNumber with MemberNumber: {user.MemberNumber}");
            if (user.MemberNumber == 2022)
            {
                return new ValidateWDNIResponse
                {
                    IsValidUser = true
                };
            }
            else
            {
                return new ValidateWDNIResponse
                {
                    IsValidUser = false
                };
            }
        }

        [HttpPost]
        [Route("terms-of-use")]
        [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public OkObjectResult TermsOfUse(TermsOfUseModelRequest user)
        {
            _logger.LogInformation($"Called StoreTermsOfUse with MemberNumber: {user.MemberNumber} and email: {user.Email}");
            return new OkObjectResult(true);
        }

        [HttpPost]
        [Route("save-user")]
        [ProducesResponseType(typeof(SaveUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public SaveUserResponse SaveUser(SaveUserRequest user)
        {
            _logger.LogInformation($"Called SaveUser with Member Number: {user.MemberNumber}");
            return new SaveUserResponse
            {
                Email = "mail@mail.com",
                Name = "Peter",
                Surname = "Jhones",
                Username = "pjones"
            };
        }

        [HttpGet]
        [Route("remote-login")]
        [ProducesResponseType(typeof(ValidateWDNIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public RemoteLoginResponse RemoteLogin([FromHeader] string signInName, [FromHeader] string password)
        {
            _logger.LogError($"Called RemoteLogin with Email: {signInName}");

            if (signInName == null || signInName == String.Empty)
                throw new Exception("Empty Email");
            if (password == null || password == String.Empty)
                throw new Exception("Empty Password");

            if (password != "Test1234!")
                throw new Exception("Fail remote Login!");

            return new RemoteLoginResponse()
            {
                Email = signInName,
                DisplayName = "Remote Dsiplay Name",
                Name = "Remote Name",
                Surname = "Remote Surname"
            };
        }

        [HttpPost]
        [Route("sameless-remote-login")]
        [ProducesResponseType(typeof(ValidateWDNIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public SamelessRemoteLoginResponse SamelessRemoteLogin(SamelessRemoteLoginRequest request)
        {
            _logger.LogError($"Called RemoteLogin with Email: {request.Email}");

            if (request.Email == null || request.Email == String.Empty)
                throw new Exception("Empty Email");
            if (request.Password == null || request.Password == String.Empty)
                throw new Exception("Empty Password");

            if (request.Password != "Test1234!")
                throw new Exception("Fail remote Login!");

            return new SamelessRemoteLoginResponse()
            {
                tokenSuccess = true,
                migrationRequired = true
            };
        }
    }
}