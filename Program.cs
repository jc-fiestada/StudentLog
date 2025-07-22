using StudentLog.Services;
using StudentLog.Models;

var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();
app.UseRouting();
app.UseStaticFiles();



app.MapPost("/validate-signin", async (HttpContext context) =>
{
    Admin? admin = null;

    // possible json corruption
    try
    {
        admin = await context.Request.ReadFromJsonAsync<Admin>();
    }
    catch (Exception)
    {
        Console.WriteLine("[DEBUG] Server error 1");

        ResponseAPI<string> error = new ResponseAPI<string>
        {
            Message = "Something went wrong from the server",
        };

        return Results.Json(error, statusCode: 500);
    }

    // possible missing fields
    if (admin?.Username is null || admin?.Password is null)
    {
        Console.WriteLine("[DEBUG] Json malformed");

        ResponseAPI<string> error = new ResponseAPI<string>
        {
            Message = "Something went wrong from user request",
        };

        return Results.Json(error, statusCode: 500);
    }

    // user input is not valid for validation

    if (admin.ErrorList().Count() != 0)
    {
        Console.WriteLine("[DEBUG] Invalid User Input");

        ResponseAPI<List<string>> error = new ResponseAPI<List<string>>
        {
            Message = "Something went wrong from the server",
            Data = admin.ErrorList()
        };

        return Results.Json(error, statusCode: 401);
    }

    AdminDbServices service = new AdminDbServices();

    bool isValidationSuccess = false;

    bool isAuthorized = service.ValidateSignIn(admin.Username, admin.Password, out isValidationSuccess);

    // if something went wrong from db extraction

    if (!isValidationSuccess)
    {
        Console.WriteLine("[DEBUG] DB error");

        ResponseAPI<List<string>> error = new ResponseAPI<List<string>>
        {
            Message = "Server database error"
        };

        return Results.Json(error, statusCode: 500);
    }

    // if user input is not the same from admin db 

    if (!isAuthorized)
    {
        Console.WriteLine("[DEBUG] Unauthorized");

        ResponseAPI<List<string>> error = new ResponseAPI<List<string>>
        {
            Message = "SignIn Unauthorized, Invalid Username or Password"
        };

        return Results.Json(error, statusCode: 401);
    }
    
    Console.WriteLine("[DEBUG] Success");

    ResponseAPI<List<string>> ok = new ResponseAPI<List<string>>
    {
        Message = "Success"
    };

    return Results.Json(ok, statusCode: 200);

});

app.Run();
