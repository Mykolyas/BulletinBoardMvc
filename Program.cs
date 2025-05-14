namespace BulletinBoardMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient(); // <== ÄÎÄÀÉ ÖÅ, ÿêùî ùå íå äîäàâ

            var app = builder.Build();

            // === ÖÅ ÌÀª ÁÓÒÈ ÄÎ ÓÑÜÎÃÎ ²ÍØÎÃÎ ===
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Ïîêàæåìî ïîâíó ïîìèëêó
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
