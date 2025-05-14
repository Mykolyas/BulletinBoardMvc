namespace BulletinBoardMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient(); // <== ����� ��, ���� �� �� �����

            var app = builder.Build();

            // === �� ��� ���� �� ������ ������ ===
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // �������� ����� �������
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Announcement}/{action=Index}/{id?}");

            app.Run();

        }
    }
}
