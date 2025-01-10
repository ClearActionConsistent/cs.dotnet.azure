using Microsoft.AspNetCore.Authorization;

namespace webapi.Authorization
{
    public class HRAdminPropationRequirement: IAuthorizationRequirement
    {
        public readonly int probationMonths;

        public HRAdminPropationRequirement(int probationMonths)
        {
            this.probationMonths = probationMonths;
        }
    }

    public class HRAdminPropationRequirementHandler : AuthorizationHandler<HRAdminPropationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRAdminPropationRequirement requirement)
        {
            if(!context.User.HasClaim("Department", "HR"))
                return Task.CompletedTask;//failed

            if (!context.User.HasClaim("Role", "Admin"))
                return Task.CompletedTask;

            if (!context.User.HasClaim(x => x.Type == "EmploymentDate"))
                return Task.CompletedTask;

            var strEmploymentDate = context.User.FindFirst(x => x.Type == "EmploymentDate")?.Value;
            var parseSuccess = DateTime.TryParse(strEmploymentDate, out DateTime employmentDate);
            
            if (!parseSuccess)
                return Task.CompletedTask;

            var employmentPeriod = DateTime.UtcNow - employmentDate;
            
            if(employmentPeriod.Days < 30 * requirement.probationMonths)
                return Task.CompletedTask;

            //only when this line of code get executed, the requirement get sactified
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
