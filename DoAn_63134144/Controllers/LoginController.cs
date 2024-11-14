using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using DoAn_63134144.Models;


namespace DoAn_63134144.Controllers
{
    public class LoginController : Controller
    {
        private DOANQLEntities14 db = new DOANQLEntities14();
        public bool Security(string value)
        {
            string kind = Session["Type"] != null ? Session["Type"].ToString() : null;
            if (value != kind) return false;
            return true;
        }
        public string getID()
        {
            // Lấy Id_user cao nhất từ cơ sở dữ liệu
            var lastId = db.USERS.ToList().Select(n => n.Id_user).Max();

            int ma = int.Parse(lastId.Substring(2)) + 1;

            var newId = "NV" + ma.ToString("D6"); // Định dạng lại chuỗi Id_user mới

            return newId;
        }

        public string getIDHr()
        {
            // Lấy Id_user cao nhất từ cơ sở dữ liệu
            var lastId = db.HRS.ToList().Select(n => n.Id_hr).Max();

            int ma = int.Parse(lastId.Substring(2)) + 1;

            var newId = "HR" + ma.ToString("D5"); // Định dạng lại chuỗi Id_user mới

            return newId;
        }

        private bool codeMatched;




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Login(string phone = "", string password = "")
        {
            var user = db.USERS.FirstOrDefault(u => u.Sdt == phone);
            if (user != null)
            {
                // Kiểm tra mật khẩu
                if (password == "123456")
                {
                    // Thực hiện các thao tác cần thiết, ví dụ: lưu thông tin đăng nhập vào session
                    Session["Id"] = user.Id_user;
                    Session["Name"] = user.Ho + " " + user.Ten_lot + " " + user.Ten;
                    Session["Type"] = user.Kind;

                    // Chuyển hướng đến trang chủ
                    return RedirectToAction("trangchuUser", "Home");
                }
            }

            var hr = db.HRS.FirstOrDefault(u => u.Sdt == phone);
            if (hr != null)
            {
                // Kiểm tra mật khẩu
                if (password == "123456")
                {
                    // Thực hiện các thao tác cần thiết, ví dụ: lưu thông tin đăng nhập vào session
                    Session["Id"] = hr.Id_hr;
                    Session["Name"] = hr.Ho + " " + hr.TenLot + " " + hr.Ten;
                    Session["Type"] = hr.Kind;

                    // Chuyển hướng đến trang chủ
                    return RedirectToAction("trangchuHR", "Home");
                }
            }
            if(phone=="admin"&& password=="01639422326hyN.")
            {
                Session["Name"] = "admin";
                Session["Type"] = 0;

                // Chuyển hướng đến trang chủ
                return RedirectToAction("trangchuAD", "Home");
            }
            //var admin = db.ADMINS.FirstOrDefault(u => u.name == phone);
            //if (admin != null)
            //{
            //    // Kiểm tra mật khẩu
            //    if (VerifyPassword(password, admin.password))
            //    {
            //        // Thực hiện các thao tác cần thiết, ví dụ: lưu thông tin đăng nhập vào session
            //        Session["Name"] = admin.name;
            //        Session["Type"] = 0;

            //        // Chuyển hướng đến trang chủ
            //        return RedirectToAction("trangchu", "Home");
            //    }
            //    ViewBag.test = admin.name;
            //}

            return View();
        }
        private bool VerifyPassword(string inputPassword, byte[] storedPasswordHash)
        {
            // Thực hiện kiểm tra so sánh mật khẩu đã băm

            // Băm mật khẩu nhập vào
            var hashedInputPassword = HashPassword(inputPassword);

            // So sánh mật khẩu đã băm
            // Trả về true nếu khớp, ngược lại trả về false
            return hashedInputPassword.SequenceEqual(storedPasswordHash);
        }

        private byte[] HashPassword(string password)
        {
            // Băm mật khẩu sử dụng thuật toán băm mạnh hơn

            // Đảm bảo rằng password đã được chuyển đổi sang binary trước khi băm

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return hashedBytes;
            }
        }
        public ActionResult DanhSachHR()
        {
            if (!Security("0")) return RedirectToAction("login", "Login");
            return View(db.HRS.ToList());
        }

        // GET: HRs/Details/5
        public ActionResult DetailsHR(string id)
        {
            if (!Security("0")) return RedirectToAction("login", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR hR = db.HRS.Find(id);
            if (hR == null)
            {
                return HttpNotFound();
            }
            return View(hR);
        }
        public ActionResult EditHR(string id)
        {
            if (!Security("0")) return RedirectToAction("login", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR hR = db.HRS.Find(id);
            if (hR == null)
            {
                return HttpNotFound();
            }
            return View(hR);
        }
        public ActionResult ChangePassHR(string id)
        {
            if (!Security("0")) return RedirectToAction("login", "Login");
            ViewBag.id = id;
            return View();
        }
        public ActionResult ConfirmChangePasswordHR(string id, string newPassword)
        {
            var hr = db.HRS.Find(id);
            hr.PasswordHash = HashPassword(newPassword);
            db.SaveChanges();
            return RedirectToAction("DanhSachHR");
        }
        public ActionResult DanhSachUSER()
        {
            if (!Security("0")) return RedirectToAction("login", "Login");
            return View(db.USERS.ToList());
        }

        // GET: USERs/Details/5
        public ActionResult DetailsUSER(string id)
        {
            if (!Security("0")) return RedirectToAction("login", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER uSER = db.USERS.Find(id);
            if (uSER == null)
            {
                return HttpNotFound();
            }
            return View(uSER);
        }
        public ActionResult EditUSER(string id)
        {
            if (!Security("0")) return RedirectToAction("login", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER uSER = db.USERS.Find(id);
            if (uSER == null)
            {
                return HttpNotFound();
            }
            return View(uSER);
        }
        public ActionResult ChangePassUSER(string id)
        {
            if (!Security("0")) return RedirectToAction("login", "Login");
            ViewBag.id = id;
            return View();
        }
        public ActionResult ConfirmChangePasswordUSER(string id, string newPassword)
        {
            var hr = db.USERS.Find(id);
            hr.PasswordHash = HashPassword(newPassword);
            db.SaveChanges();
            return RedirectToAction("DanhSachUSER");
        }
        public ActionResult Logout()
        {
            // Xóa thông tin đăng nhập từ session
            Session.Clear();

            // Chuyển hướng đến trang đăng nhập
            return RedirectToAction("trangchu", "Home");
        }
        
        public ActionResult Register()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }
        [HttpPost]
        public ActionResult SendCode(string Email = "",string PhoneNumber="")
        {
            // Kiểm tra xem một trong hai trường đã tồn tại trong cơ sở dữ liệu hay chưa
            bool isEmailExists = CheckEmailExists(Email);
            bool isPhoneNumberExists = CheckPhoneNumberExists(PhoneNumber);

            if (isEmailExists || isPhoneNumberExists)
            {
                // Thông báo rằng địa chỉ email hoặc số điện thoại đã tồn tại
                TempData["Message"] = "Địa chỉ email hoặc số điện thoại đã tồn tại.";
                return RedirectToAction("Register", "Login");
            }

            if (ModelState.IsValid)
            {
                // Tạo số ngẫu nhiên 6 chữ số
                Random random = new Random();
                int code = random.Next(100000, 999999);

                // Gửi email chứa mã code
                string email = Email;
                string subject = "Mã xác nhận";
                string message = $"Mã xác nhận của bạn là: {code}";

                SendEmail(email, subject, message);

                Session["Code"] = code;
                Session["sdt"] = PhoneNumber;
                Session["email"] = Email;

                return RedirectToAction("VerifyCode", new { email = email });
            }

            return View("Register", Email);
        }
        private bool CheckEmailExists(string email)
        {
            // Kết nối đến cơ sở dữ liệu
            
                // Kiểm tra xem email đã tồn tại trong bảng USERS hay chưa
                bool isEmailExists = db.USERS.Any(u => u.Email == email);

                return isEmailExists;
        }

        private bool CheckPhoneNumberExists(string phoneNumber)
        {
                // Kiểm tra xem số điện thoại đã tồn tại trong bảng USERS hay chưa
            bool isPhoneNumberExists = db.USERS.Any(u => u.Sdt == phoneNumber);

            return isPhoneNumberExists;
           
        }
        private void SendEmail(string email="", string subject="", string message = "")
        {
            
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.From = new System.Net.Mail.MailAddress("hung.nbn.63cntt@ntu.edu.vn");
            mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = message;
            //
            //
            mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new System.Net.NetworkCredential("hung.nbn.63cntt@ntu.edu.vn", "225774478");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
        public ActionResult VerifyCode(string email)
        {
            // Hiển thị view để người dùng nhập mã code từ email
            return View(new VerifyCodeViewModel { Email = email });
        }

        [HttpPost]

        public ActionResult VerifyCode(VerifyCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
               

                if (model.Code == Session["Code"].ToString())
                {
                    // Mã code hợp lệ, hoàn tất quá trình đăng ký
                    return RedirectToAction("Success");
                }
                else
                {
                    ModelState.AddModelError("", "Mã code không hợp lệ.");
                }
            }

            return View(model);
        }
        public ActionResult Success()
        {
            string Id_max = getID();
            ViewBag.Id_max = Id_max;
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Success([Bind(Include = "Ho,Ten_lot,Ten,Sdt,Email,Dia_chi,Ngsinh,Gtinh")] USER uSER)
        {
            if (ModelState.IsValid)
            {
                uSER.Id_user = getID();
                uSER.Luong = 0;
                uSER.Kind = 1;
                uSER.Point = 0;
                uSER.Bad = 0;
                Random rnd = new Random();
                int pass = rnd.Next(1000000, 100000000);
                uSER.PasswordHash = HashPassword(pass.ToString());
                db.USERS.Add(uSER);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return RedirectToAction("trangchu","Home");
        }
        public ActionResult InfoUser()
        {
            if (!Security("1")) return RedirectToAction("login", "Login");
            if (TempData["SuccessChangePasswordMessage"] != null)
            {
                ViewBag.Message = TempData["SuccessChangePasswordMessage"].ToString();
            }
            if (TempData["ErrorChangeMessage"] != null)
            {
                ViewBag.Message = TempData["ErrorChangeMessage"].ToString();
            }
            string id = Session["Id"].ToString();
            var user = db.USERS.Find(id);
            return View(user);
        }
        public ActionResult ChangeBank(string bank="",string stk ="")
        {
            if (!Security("1")) return RedirectToAction("login", "Login");
            if (bank == "" || stk == "") return View();
            string id = Session["Id"].ToString();
            var user = db.USERS.Find(id);
            user.Bank = bank;
            user.Stk = stk;
            db.SaveChanges();
            return RedirectToAction("InfoUser", "Login");
        }
        public ActionResult InfoHr()
        {
            if (!Security("2")) return RedirectToAction("login", "Login");
            if (TempData["SuccessChangePasswordMessage"] != null)
            {
                ViewBag.Message = TempData["SuccessChangePasswordMessage"].ToString();
            }
            if (TempData["ErrorChangeMessage"] != null)
            {
                ViewBag.Message = TempData["ErrorChangeMessage"].ToString();
            }
            string id = Session["Id"].ToString();
            var hr = db.HRS.Find(id);
            return View(hr);
        }
        public ActionResult ChangePasswordWithPassword()
        {
            if (!(Security("1")|| Security("2"))) return RedirectToAction("login", "Login");
            return View();
        }
        public ActionResult ConfirmChangePassword(string currentPassword, string newPassword)
        {
            string id = Session["Id"].ToString();
            if (Session["Type"].ToString() == "1")
            {
                USER user = db.USERS.Find(id);
                if (VerifyPassword(currentPassword, user.PasswordHash))
                {
                    // Thực hiện thay đổi mật khẩu
                    user.PasswordHash = HashPassword(newPassword);
                    db.SaveChanges();

                    TempData["SuccessChangePasswordMessage"] = "Mật khẩu thay đổi thành công.";
                    return RedirectToAction("InfoUser");
                }
                else
                {
                    TempData["ErrorChangeMessage"] = "Mật khẩu hiện tại không đúng.";
                    return RedirectToAction("Error", "ControllerName");
                }
            }
            if (Session["Type"].ToString() == "2")
            {
                HR hr = db.HRS.Find(id);
                if (VerifyPassword(currentPassword, hr.PasswordHash))
                {
                    // Thực hiện thay đổi mật khẩu
                    hr.PasswordHash = HashPassword(newPassword);
                    db.SaveChanges();

                    TempData["SuccessChangePasswordMessage"] = "Mật khẩu thay đổi thành công.";
                    return RedirectToAction("InfoHr");
                }
                else
                {
                    TempData["ErrorChangeMessage"] = "Mật khẩu hiện tại không đúng.";
                    return RedirectToAction("Error", "ControllerName");
                }
            }
            return View();
        }
        public ActionResult CreateHr()
        {
            if (!Security("0")) return RedirectToAction("login", "Login");
            return View();
        }
        public ActionResult ConfirmCreateHr([Bind(Include = "Id_hr,Ho,TenLot,Ten,Ten_KS,Star,Sdt,Email,Diachi,Thanhtoan,Kind,images")] HR hR, HttpPostedFileBase images)
        {
            if (!Security("0")) return RedirectToAction("login", "Login");
            if (ModelState.IsValid)
            {
                // Lưu tệp ảnh vào thư mục "images"
                if (images != null && images.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(images.FileName);
                    string path = Path.Combine(Server.MapPath("~/images"), fileName);
                    images.SaveAs(path);

                    // Gán tên tệp ảnh cho hR.images
                    hR.images = fileName;
                }
                Random rnd = new Random();
                int pass = rnd.Next(1000000, 100000000);
                hR.PasswordHash = HashPassword(pass.ToString());
                hR.Id_hr = getIDHr();
                hR.Thanhtoan = 0;
                hR.Kind = 2;
                db.HRS.Add(hR);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View();
        }

    }
}
