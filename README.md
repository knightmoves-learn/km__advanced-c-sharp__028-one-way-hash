# 025 Role Based Authentication

## Lecture

[![# Role Based Authentication](https://img.youtube.com/vi/RTUTtORZ8nM/0.jpg)](https://www.youtube.com/watch?v=RTUTtORZ8nM)

## Instructions

The classes `UserDto`, `User`, `IUserRepository`, and `UserRepository` have been created and can be found in `HomeEnergyApi/Models`

In `HomeEnergyApi/Controllers/AuthenticationController.cs`
- Add three private readonly properties on `AuthenticationController` with the following names / types
    - userRepository / `IUserRepository`
    - passwordHasher / `ValueHasher`
    - mapper / `IMapper`
- Add the newly created properties as arguments to the constructor on `AuthenticationController`
- Create a new public async method `Register()` returning a `Task<IActionResult>`
    - `Register()` should have an `HttpPost` attribute with the route `register`
    - `Register()` should have one argument of type `UserDto` coming from the body of the Http request
    - `Register()` should create a variable from the result of calling `FindByUsername` on the newly created `IUserRepository` property and passing the `Username` property on the passed `UserDto`
        - If the newly created variable is null, `Register()` should return `BadRequest("Username is already taken.")`
    - `Register()` should create a variable calling `.Map<User>()` on the newly created `IMapper` property passing the passed `UserDto`
        - `Register()` should set the `HashedPassword` property on this newly created variable to the result of calling `HashPassword()` on the newly passed `ValueHasher` property and passing the `Password` property on the passed `UserDto`
        - `Register()` should call `Save()` on the newly created `IUserRepository` property passing this newly created variable
    - `Register()` should return `Ok("User registered successfully.")`
- Refactor `Token()` so that...
    - The argument being passed is now type `UserDto`
    - If the result of calling `FindByUsername()` on the newly created `IUserRepository` and passing the `UserDto` argument is null OR the result of calling `VerifyPasssword()` on the newly created `ValueHasher` property and passing the `Username` and `Password` from the passed `UserDto` is false
        - Return `Unauthorized("Invalid username or password.");`
    - The result of calling `FindByUsername()` on the newly created `IUserRepository` and passing the `UserDto` argument is passed into `GenerateJwtToken()`
- Refactor `GenerateJwtToken()` so that...
    - The argument being passed is now type `User`
    - In the `claims[]` being created, the hardcoded email is replaced with the `Username` on the passed `User` and the formerly passed `string` role is replaced with the `Role` on the passed `User`

In `HomeEnergyApi/Dtos/HomeProfile.cs`
- Call `CreateMap()` supplying the types `UserDto` and `User`

In `HomeEnergyApi/Security/ValueHasher`
- Create a new public class `ValueHasher`
    - Create a `HashPassword()` method taking one argument of type `string`
        - Using a variable of type `SHA256`, create a variable from the result of calling `SHA256.ComputeHash` and passing `Encoding.UTF8.GetBytes()` with the `string` argument passed
        - Return the result of passing this created variable to `Convert.ToBase64String()`
    - Create a `VerifyPassword()` method taking two arguments of type `string`
        - Create a variable from the result of calling `HashPassword()` on the second argument
        - If the created variable and the first argument are the same, return true. Otherwise, return false

In `HomeEnergyApi/Models/HomeDbContext.cs`
- Add a new public `DbSet<User>` named "Users"

In `HomeEnergyApi/Program.cs`
- Add a scoped service supplying the types `IUserRepository` and `UserRepository`
- Add a singleton service supplying the type `ValueHasher`

In your terminal
- ONLY IF you are working on codespaces or a different computer/environment as the previous lesson and don't have `dotnet-ef` installed globally, run `dotnet tool install --global dotnet-ef`, otherwise skip this step
    - To check if you have `dotnet-ef` installed, run `dotnet-ef --version`
- Run `dotnet ef migrations add AddUserAuthenticationAndHashing`
- Run `dotnet ef database update`

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
- https://en.wikipedia.org/wiki/Cryptographic_hash_function

Copyright &copy; 2025 Knight Moves. All Rights Reserved.
