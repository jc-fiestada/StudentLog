using StudentLog.Services;
using StudentLog.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();



var app = builder.Build();
app.UseSession();
app.UseRouting();
app.UseStaticFiles();

// Update specific student

app.MapPost("/update-student", async (HttpContext context) =>
{
    // i kinda did copy pasted some parts here instead of making a reusable method instead but i am still not confident
    // with httpcontext as a parameter so i might have to learn how to handle this properly later on
    // and yeah i do kinda already want to move on to the next topic/project

    Student? student;

    // checks if json was able to be stored in student object, if not it might be corrupted
    try
    {
        student = await context.Request.ReadFromJsonAsync<Student>();
    }
    catch (Exception)
    {
        Console.WriteLine("[DEBUG] malformed json 1");
        ResponseAPI<string> error = new ResponseAPI<string>
        {
            Message = "Corrupted or malformed json has been detected from client"
        };

        return Results.Json(error, statusCode: 400);
    }

    // if null values have been detected, json must be missing a value or incomplete

    if (student?.Name is null || student?.Sex is null || student?.BirthDate is null)
    {
        Console.WriteLine("[DEBUG] malformed json 2");
        ResponseAPI<string> error = new ResponseAPI<string>
        {
            Message = "Corrupted, malformed, or a missing value from json has been detected from client"
        };

        return Results.Json(error, statusCode: 400);
    }

    // checks if values are acceptable for validation

    if (student.ErrorList().Count() != 0)
    {
        Console.WriteLine("[DEBUG] invalid user input");
        ResponseAPI<List<string>> error = new ResponseAPI<List<string>>
        {
            Message = "Invalid user input has been detected",
            Data = student.ErrorList()
        };

        return Results.Json(error, statusCode: 422);
    }

    StudentDbServices service = new StudentDbServices();
    bool IsUpdated;

    try
    {
        service.UpdateStudent(student, out IsUpdated);
    }
    catch (Exception)
    {
        Console.WriteLine("[DEBUG] db error");
        ResponseAPI<string> error = new ResponseAPI<string>
        {
            Message = "Issue has been detected on servers database"
        };

        return Results.Json(error, statusCode: 500);
    }



    if (!IsUpdated)
    {
        Console.WriteLine("[DEBUG] db error, student was not updated");
        ResponseAPI<string> error = new ResponseAPI<string>
        {
            Message = "Failed to update student"
        };

        return Results.Json(error, statusCode: 500);
    }
Console.WriteLine("[DEBUG] success");
    
    ResponseAPI<string> ok = new ResponseAPI<string>
    {
        Message = "Student has been successfully updated"
    };

    return Results.Json(ok, statusCode: 200);

});

// deletes specific student

// feels like crappy code, i rushed this part
app.MapPost("/delete-student", async (HttpContext context) =>
{
    string? name;

    try
    {
        DeleteRequestName? RequestName = await context.Request.ReadFromJsonAsync<DeleteRequestName>();
        name = RequestName?.Name;
    }
    catch (Exception)
    {
        ResponseAPI<string> error = new ResponseAPI<string>
        {
            Message = "Malformed json detected"
        };

        return Results.Json(error, statusCode: 400);
    }

    if (name is null)
    {
        ResponseAPI<string> error = new ResponseAPI<string>
        {
            Message = "Malformed json detected"
        };

        return Results.Json(error, statusCode: 400);
    }

    bool IsDeleted;

    try
    {
        StudentDbServices services = new StudentDbServices();

        services.DeleteStudent(name, out IsDeleted);
    }
    catch (Exception)
    {
        ResponseAPI<string> error = new ResponseAPI<string>
        {
            Message = "Issu has been detected on servers database"
        };

        return Results.Json(error, statusCode: 500);
    }


    if (!IsDeleted)
    {
        ResponseAPI<string> error = new ResponseAPI<string>
        {
            Message = "Student has not been found on the system"
        };

        return Results.Json(error, statusCode: 422);
    }
    
    ResponseAPI<string> ok = new ResponseAPI<string>
    {
        Message = "Student has been successfully deleted"
    };

    return Results.Json(ok, statusCode: 200);

});

app.MapGet("/select-all-student", (HttpContext context) =>
{
    StudentDbServices services = new StudentDbServices();
    List<Student> student = services.ViewStudent();

    ResponseAPI<List<Student>> response = new ResponseAPI<List<Student>>
    {
        Message = "current student database data",
        Data = student
    };

    return Results.Json(response, statusCode: 200);

});

// INSERT STUDENT TO DB
app.MapPost("/insert-student", async (HttpContext context) =>
{
    Student? student;

    // checks if json was able to be stored in student object, if not it might be corrupted
    try
    {
        student = await context.Request.ReadFromJsonAsync<Student>();
    }
    catch (Exception)
    {
        Console.WriteLine("[DEBUG] malformed json 1");
        ResponseAPI<string> error = new ResponseAPI<string>
        {
            Message = "Corrupted or malformed json has been detected from client"
        };

        return Results.Json(error, statusCode: 400);
    }

    // if null values have been detected, json must be missing a value or incomplete

    if (student?.Name is null || student.Sex is null || student.BirthDate is null)
    {
        Console.WriteLine("[DEBUG] malformed json 2");
        ResponseAPI<string> error = new ResponseAPI<string>
        {
            Message = "Corrupted, malformed, or a missing value from json has been detected from client"
        };

        return Results.Json(error, statusCode: 400);
    }

    // checks if values are acceptable for validation

    if (student.ErrorList().Count() != 0)
    {
        Console.WriteLine("[DEBUG] invalid user input");
        ResponseAPI<List<string>> error = new ResponseAPI<List<string>>
        {
            Message = "Invalid user input has been detected",
            Data = student.ErrorList()
        };

        return Results.Json(error, statusCode: 422);
    }

    StudentDbServices service = new StudentDbServices();

    // If db related issue suddenly happens
    try
    {
        service.InsertStudent(student);
    }
    catch (Exception)
    {
        Console.WriteLine("[DEBUG] db error");
        ResponseAPI<string> error = new ResponseAPI<string>
        {
            Message = "DB related error"
        };

        return Results.Json(error, statusCode: 500);
    }

    // success bruh
    Console.WriteLine("[DEBUG] Success");
    ResponseAPI<string> ok = new ResponseAPI<string>
    {
        Message = "Success"
    };

    return Results.Json(ok, statusCode: 200);
});


// AUTHENTICATE ADMIN ACTIVE SESSION
// ts only works (or atleast just being used only) in dom, learn how to make ts into a function so i can reuse ts


app.MapGet("/validate-admin-session", (HttpContext context) =>
{
    int? inSession = context.Session.GetInt32("isInSession");

    if (inSession is null)
    {
        ResponseAPI<string> response = new ResponseAPI<string>()
        {
            Message = "Unauthorized Access Detected"
        };

        Console.WriteLine("[DEBUG] unauthorized access 1");

        return Results.Json(response, statusCode: 401);
    }

    if (inSession != 1)
    {
        ResponseAPI<string> response = new ResponseAPI<string>()
        {
            Message = "Unauthorized Access Detected"
        };

        Console.WriteLine("[DEBUG] unauthorized access 2");

        return Results.Json(response, statusCode: 401);
    }

    ResponseAPI<string> ok = new ResponseAPI<string>()
    {
        Message = "Authorized"
    };

    Console.WriteLine("[DEBUG] authorized");

    return Results.Json(ok, statusCode: 200);
});


// SIGN IN ATTEMPT


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
            Message = "Invalid User Input",
            Data = admin.ErrorList()
        };

        return Results.Json(error, statusCode: 422);
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
            Message = "Sign-In Unauthorized, Invalid Username or Password"
        };

        return Results.Json(error, statusCode: 401);
    }
    
    Console.WriteLine("[DEBUG] Success");

    ResponseAPI<List<string>> ok = new ResponseAPI<List<string>>
    {
        Message = "Success"
    };

    context.Session.SetInt32("isInSession", 1);

    return Results.Json(ok, statusCode: 200);

});

app.Run();


record DeleteRequestName(string Name);