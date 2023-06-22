namespace Sistema_Control_Seguimiento_Backend.Token
{
    public class UserSession : IUserSession
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserSession(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int GetUserSessionId()
        {
            var user = httpContextAccessor.HttpContext.User;

            string employeeIdStr = null;

            if (user.Claims != null)
            {
                employeeIdStr = user?.Claims?.FirstOrDefault(u => u.Type == "idUsuario")?.Value;
            }

            return employeeIdStr is null ? 0 : Int32.Parse(employeeIdStr);


        }
    }
}
