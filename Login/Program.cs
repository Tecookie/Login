using System.Text;
using BussinessLayer.IServices;
using BussinessLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var secretKey = "the-key-size-must-be-greater-than-512-bits-to-make-a-secret-key-random-word-random-word"; //Cái này thường sẽ dấu ở UserSecret
var secretKeyByte = Encoding.UTF8.GetBytes(secretKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Hệ thống không quan tâm token được đến từ đâu
            ValidateAudience = false, // Hệ thống không cần biết đối tượng là ai
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKeyByte), //Kiểm tra chữ ký hợp lệ
            ClockSkew = TimeSpan.Zero //Đặt thời gian khi nào hết hạn

            // Trường hợp để true để thêm bảo mật
            // ValidIssuer = "https://my-auth-server.com", 
            // ValidAudience = "my-awesome-api"              
        }; // Này là cấu hình JWT
    });

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
