using System.Diagnostics;
using System.Globalization;
using KendoCrudMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


namespace kendoCrudMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Datalayer _dataLayer;

        public HomeController(ILogger<HomeController> logger, Datalayer dataLayer)
        {
            _logger = logger;
            _dataLayer = dataLayer;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(UserModel user)
        {
            /*if (!ModelState.IsValid)
            {
                return View(user);
            }*/

            // Format the date of birth
            user.DateOfBirth = DateTime.ParseExact(user.DateOfBirth.ToString(), "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            try
            {
                string sql;
                if (user.UserId > 0)
                {
                    sql = "UPDATE tbluser SET email = @Email, mobile = @Mobile, password = @Password, dob = @Dob WHERE userid = @UserId";
                    _dataLayer.ExecuteQuery(sql, new SqlParameter("@Email", user.Email),
                                                    new SqlParameter("@Mobile", user.Mobile),
                                                    new SqlParameter("@Password", user.Password),
                                                    new SqlParameter("@Dob", user.DateOfBirth),
                                                    new SqlParameter("@UserId", user.UserId));
                }
                else
                {
                    sql = "INSERT INTO tbluser (email, mobile, password, dob) VALUES (@Email, @Mobile, @Password, @Dob)";
                    _dataLayer.ExecuteQuery(sql, new SqlParameter("@Email", user.Email),
                                                    new SqlParameter("@Mobile", user.Mobile),
                                                    new SqlParameter("@Password", user.Password),
                                                    new SqlParameter("@Dob", user.DateOfBirth));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while executing the query: " + ex.Message.ToString());
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetAllUsers()
        {
            List<UserModel> userList = _dataLayer.GetUsers();
            return Json(userList);
        }

        [HttpPost]
        public JsonResult Delete(int userId)
        {
            string sql = "DELETE FROM tbluser WHERE userid = @UserId";
            _dataLayer.ExecuteQuery(sql, new SqlParameter("@UserId", userId));
            return Json(new { success = true });
        }
    }
}
