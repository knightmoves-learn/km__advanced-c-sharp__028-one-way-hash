# 025 Role Based Authentication

## Lecture

[![# Role Based Authentication](https://img.youtube.com/vi/RTUTtORZ8nM/0.jpg)](https://www.youtube.com/watch?v=RTUTtORZ8nM)

## Instructions

In `HomeEnergyApi/Program.cs`
- Add an option to the call to `AddAuthorization` to create a policy named "AdminOnly" and which requires the role "Admin"
- Add an option to the call to `AddAuthorization` to create a policy named "UserOnly" and which requires the role "User"

In `HomeEnergyApi/Controllers/AuthenticationController.cs`
- Add an argument to the `Token()` method, which gets a `TokenDto` from the body of the HTTP request
- Add logic to the `Token()` method, that will return `BadRequest("Role is required.")` from the method if the `Role` property on the passed `TokenDto` is null or empty
- Pass the `Role` property on the passed `TokenDto` to the call to `GenerateJwtToken()`
- Add an argument of type `string` to `GenerateJwtToken()`
- Inside of `GenerateJwtToken()` add a new `Claim` to the array holding other `Claim`s, passing `ClaimTypes.Role` and the newly created `string` argument into it's constructor

In `HomeEnergyApi/Dtos/TokenDto.cs`
- Create a public class `TokenDto` with a public property `Role` of type `string`

In `HomeEnergyApi/Controllers/HomeAdminController.cs`
- Modify the Authorize attribute on the `CreateHome()` method to set it's policy to "AdminOnly"

In `HomeEnergyApi/Controllers/HomeController.cs`
- Add authorization to the `Bang()` method with the policy set to "UserOnly"


## Additional Information
- Do not remove or modify anything in `HomeEnergyApi.Tests`
- Some Models may have changed for this lesson from the last, as always all code in the lesson repository is available to view
- Along with `using` statements being added, any packages needed for the assignment have been pre-installed for you, however in the future you may need to add these yourself

## Building toward CSTA Standards:
- Give examples to illustrate how sensitive data can be affected by malware and other attacks (3A-NI-05) https://www.csteachers.org/page/standards
- Recommend security measures to address various scenarios based on factors such as efficiency, feasibility, and ethical impacts (3A-NI-06) https://www.csteachers.org/page/standards
- Compare various security measures, considering tradeoffs between the usability and security of a computing system (3A-NI-07) https://www.csteachers.org/page/standards
- Explain tradeoffs when selecting and implementing cybersecurity recommendations (3A-NI-08) https://www.csteachers.org/page/standards
- Compare ways software developers protect devices and information from unauthorized access (3B-NI-04) https://www.csteachers.org/page/standards
- Explain security issues that might lead to compromised computer programs (3B-AP-18) https://www.csteachers.org/page/standards

## Resources
- https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-9.0

Copyright &copy; 2025 Knight Moves. All Rights Reserved.
