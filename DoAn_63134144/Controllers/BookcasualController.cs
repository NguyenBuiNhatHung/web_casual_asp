using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn_63134144.Models;

namespace DoAn_63134144.Controllers
{
    public class BookcasualController : Controller
    {
        private DOANQLEntities14 db = new DOANQLEntities14();

        // GET: Bookcasual
        public string getID()
        {
            // Lấy Id_user cao nhất từ cơ sở dữ liệu
            var lastId = db.BOOKS.ToList().Select(n => n.Id_book).Max();

            int ma = int.Parse(lastId.Substring(2)) + 1;

            var newId = "BO" + ma.ToString("D7"); // Định dạng lại chuỗi Id_user mới

            return newId;
        }
        public string getIDJob()
        {
            // Lấy Id_user cao nhất từ cơ sở dữ liệu
            var lastId = db.JOBS.ToList().Select(n => n.Id_job).Max();
            int ma;
            if (lastId != null)
            {
                ma = int.Parse(lastId.Substring(2)) + 1;
            }
            else ma = 1;
            

            var newId = "JO" + ma.ToString("D11"); // Định dạng lại chuỗi Id_user mới

            return newId;
        }
        public bool Security(string value)
        {
            string kind = Session["Type"] != null ? Session["Type"].ToString() : null;
            if(value != kind) return false;
            return true;
        }
        public ActionResult Index()
        {
            if (!Security("2")) return RedirectToAction("login", "Login");
            DateTime currentDate = DateTime.Now.Date;
            string id = Session["Id"].ToString();
            var book = db.BOOKS.Where(b => b.Id_hr == id && b.Ngay >= currentDate).ToList();
            return View(book);
        }

        // GET: Bookcasual/Details/5

        // GET: Bookcasual/Create
        public ActionResult Create()
        {
            if (!Security("2")) return RedirectToAction("login", "Login");
            return View();
        }

        // POST: Bookcasual/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_hr,Ngay,Tstart,Tend,Price,Support,Location,Note")] BOOK bOOK)
        {
            if (!Security("2")) return RedirectToAction("login", "Login");
            if (ModelState.IsValid)
            {
                bOOK.Id_book= getID();
                db.BOOKS.Add(bOOK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bOOK);
        }

        // GET: Bookcasual/Edit/5
        public ActionResult Edit(string id)
        {
            if (!Security("2")) return RedirectToAction("login", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOK bOOK = db.BOOKS.Find(id);
            if (bOOK == null)
            {
                return HttpNotFound();
            }
            return View(bOOK);
        }

        // POST: Bookcasual/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_book,Id_hr,Ngay,Tstart,Tend,Price,Support,Location,Note")] BOOK bOOK)
        {
            if (!Security("2")) return RedirectToAction("login", "Login");
            if (ModelState.IsValid)
            {
                db.Entry(bOOK).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bOOK);
        }
        public ActionResult DeleteJob(string id)
        {
            if (!Security("2")) return RedirectToAction("login", "Login");
            string type = Session["Type"].ToString();
            JOB jOB = db.JOBS.Find(id);
            db.JOBS.Remove(jOB);
            db.SaveChanges();
            return RedirectToAction("JobDangKy");
        }

        // POST: Job/Delete/5

        public ActionResult DeleteByHr(string id)
        {
            if (!Security("2")) return RedirectToAction("login", "Login");
            JOB jOB = db.JOBS.Find(id);

            ///gửi thông tin hủy ca về casual
            ///
            /// 
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.From = new System.Net.Mail.MailAddress("hung.nbn.63cntt@ntu.edu.vn");
            mail.To.Add(jOB.USER.Email);
            mail.Subject = "Hủy ca làm";
            mail.Body = "hủy ca ngày" + jOB.Ngay + ", ca làm :" + jOB.Tstart + " đến :" + jOB.Tend + " ở :" + jOB.HR.Ten_KS;
   
            mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new System.Net.NetworkCredential("hung.nbn.63cntt@ntu.edu.vn", "225774478");
            smtp.EnableSsl = true;
            smtp.Send(mail);

            /// 
            ///
            db.JOBS.Remove(jOB);
            db.SaveChanges();
            return RedirectToAction("Danhsach");
        }

        // POST: Job/Delete/5

        // GET: Bookcasual/Delete/5
        public ActionResult Delete(string id)
        {
            if (!Security("2")) return RedirectToAction("login", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOK bOOK = db.BOOKS.Find(id);
            if (bOOK == null)
            {
                return HttpNotFound();
            }
            return View(bOOK);
        }

        // POST: Bookcasual/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (!Security("2")) return RedirectToAction("login", "Login");
            BOOK bOOK = db.BOOKS.Find(id);
            db.BOOKS.Remove(bOOK);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //// đăng ký
        ////
        ///
        public ActionResult Dangky()
        {
            if (!Security("1")) return RedirectToAction("login", "Login");
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            if (TempData.ContainsKey("ErrorMessage"))
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
            }
            DateTime currentDate = DateTime.Now.Date;
            var book = db.BOOKS.Where(b => b.Ngay >= currentDate).OrderBy(b => b.Ngay).ToList();
            return View(book);
        }
        public ActionResult DangkyAD()
        {
            if (!Security("0")) return RedirectToAction("login", "Login");
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            if (TempData.ContainsKey("ErrorMessage"))
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
            }
            DateTime currentDate = DateTime.Now.Date;
            var book = db.BOOKS.Where(b => b.Ngay >= currentDate).OrderBy(b => b.Ngay).ToList();
            return View(book);
        }
        public HttpRequestBase GetRequest()
        {
            return Request;
        }



        public ActionResult CreateJob([Bind(Include = "Id_job,Id_hr,Ngay,Tstart,Tend,Price,Support,Sta,Pay,Id_user,Note,Id_change,Status_change,Location")] JOB jOB,
                               string Id_hr, string Ngay, string Tstart, string Tend, string Price, string Support, string Note,string Location)
        {
            if (!Security("1")) return RedirectToAction("login", "Login");
            var userID = Session["Id"].ToString();
            TimeSpan startTime = TimeSpan.Parse(Tstart);
            TimeSpan endTime = TimeSpan.Parse(Tend);
            DateTime ngay = DateTime.Parse(Ngay);
            var check = db.JOBS.Where(c => c.Ngay== ngay && c.Id_user == userID && c.Location==Location && c.Id_hr == Id_hr && c.Tstart == startTime && c.Tend == endTime);
            if (!check.Any())
            {
                jOB.Id_job = getIDJob();
                jOB.Id_hr = Id_hr;
                jOB.Ngay = DateTime.Parse(Ngay);
                jOB.Tstart = TimeSpan.Parse(Tstart);
                jOB.Tend = TimeSpan.Parse(Tend);
                jOB.Price = Decimal.Parse(Price);
                jOB.Support = Decimal.Parse(Support);
                jOB.Location = Location;
                jOB.Sta = "0";
                jOB.Pay = "0";
                jOB.Id_user = userID;
                jOB.Note = Note;
                jOB.Id_change = "";
                jOB.Status_change = "";

                db.JOBS.Add(jOB);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Đăng ký thành công.";
                return RedirectToAction("Dangky");
            }

            TempData["ErrorMessage"] = "Đăng ký thất bại.";
            return RedirectToAction("Dangky");
        }

        public ActionResult JobDangKy()
        {
            if (!Security("1")) return RedirectToAction("login", "Login");
            DateTime currentDate = DateTime.Now.Date;
            string id = Session["Id"].ToString();
            var job = db.JOBS.Where(j => j.Ngay >= currentDate && j.Id_user==id && j.Sta=="0").OrderBy(j => j.Ngay).ToList();
            return View(job);
        }

        public ActionResult LichLamViec()
        {
            if (!Security("1")) return RedirectToAction("login", "Login");
            DateTime currentDate = DateTime.Now.Date;
            string id = Session["Id"].ToString();
            var job = db.JOBS.Where(j => j.Ngay >= currentDate && j.Id_user == id && j.Sta=="1").OrderBy(j => j.Ngay).ToList();
            return View(job);
        }

        public ActionResult XacNhanDangKy()
        {
            if (!Security("2")) return RedirectToAction("login", "Login");
            TempData["SuccessMessage"] = "";
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            if (TempData.ContainsKey("ErrorMessage"))
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
            }
            DateTime currentDate = DateTime.Now.Date;
            string id = Session["Id"].ToString();
            string admin = Session["Type"].ToString();
            var job = db.JOBS.Where(j => j.Ngay >= currentDate && (j.Id_hr == id || admin =="0") && j.Sta == "0");
            return View(job);
        }
        
        public ActionResult XacNhan(string id)
        {
            var check = db.JOBS.Find(id);
            var job =db.JOBS.Where(j => j.Sta == "1" && j.Id_job !=id && j.Ngay == check.Ngay && j.Id_user==check.Id_user &&
                                   ((check.Tend > j.Tstart && j.Tend > check.Tstart) ||
                                     (check.Tstart >= j.Tstart && check.Tstart < j.Tend) ||
                                        (check.Tend > j.Tstart && check.Tend <= j.Tend))    );
            if(!job.Any())
            {
                check.Sta = "1";
                db.SaveChanges();
                TempData["SuccessMessage"] = "xác nhận thành công";
                return RedirectToAction("XacNhanDangKy");
            }
            TempData["ErrorMessage"] = "Công nhật bị trùng lịch với những công việc khác";
            return RedirectToAction("XacNhanDangKy");
        }

        public ActionResult Danhsach()
        {
            if (!Security("2")) return RedirectToAction("login", "Login");
            string id = Session["Id"].ToString();
            DateTime currentDate = DateTime.Today;

            var jobs = db.JOBS.Where(j => (j.Id_hr == id) && j.Sta == "1" && j.Ngay > currentDate).OrderBy(j => j.Ngay).ToList();

            return View(jobs);
        }
        public ActionResult ChangeTime(string id)
        {
            if (!Security("2")) return RedirectToAction("login", "Login");
            ViewBag.id = id;
            return View();
        }
        public ActionResult ConfirmChangeTime(string id, TimeSpan newStart, TimeSpan newEnd)
        {
            var job = db.JOBS.Find(id);
            job.Tstart = newStart; job.Tend = newEnd;
            db.SaveChanges();
            return RedirectToAction("DanhSach");
        }
        public ActionResult DanhsachAD()
        {
            if (!Security("0")) return RedirectToAction("login", "Login");
            DateTime currentDate = DateTime.Today;

            var jobs = db.JOBS.Where(j =>j.Sta == "1" && j.Ngay > currentDate).OrderBy(j => j.Ngay).OrderBy(j=>j.Id_hr).ToList();
            return View(jobs);
        }

        public ActionResult ChuyenCa(string id)
        {
            if (!Security("1")) return RedirectToAction("login", "Login");
            ViewBag.Id_job = id;
            return View();
        }

        public ActionResult ConFirmChuyenCa(string Id_job, string Id_change ="")
        {
            var job = db.JOBS.Find(Id_job);
            if(Id_change =="")
            {
                job.Status_change = "1";
                db.SaveChanges();
            }
            else
            {
                var user = db.USERS.Find(Id_change);
                if(user == null)
                {
                    TempData["ErrorChuyenCa"] = "Không Tìm thấy Id người dùng";
                    return RedirectToAction("LichLamViec");
                }
                else
                {
                    if(user.Bad >1|| (user.Gtinh=="1"&&job.USER.Gtinh=="0"))
                    {
                        TempData["ErrorChuyenCa"] = "Người dùng không đủ điều khiện để chuyển ca";
                        return RedirectToAction("LichLamViec");
                    }
                    else
                    {
                        job.Id_change = Id_change;
                        job.Status_change =null;
                        db.SaveChanges();
                        TempData["ErrorChuyenCa"] = "Chuyển ca thành công, đợi người nhận ca xác nhận";
                        return RedirectToAction("LichLamViec");
                    }
                }
            }
            TempData["ErrorChuyenCa"] = "Chuyển ca lên trang nhượng ca thành công";
            return RedirectToAction("LichLamViec");
        }

        public ActionResult NhuongCa(String id_job ="")
        {
            if (!Security("1")) return RedirectToAction("login", "Login");
            string id_user = Session["Id"].ToString();
            DateTime currentDate = DateTime.Now.Date;
            if (id_job != "")
            {
                var user = db.USERS.Find(id_user);
                var job = db.JOBS.Find(id_job);
                if (user.Bad > 1 || (user.Gtinh == "1" && job.USER.Gtinh == "0"))
                {
                    TempData["ErrorChuyenCa"] = "Người dùng không đủ điều khiện để nhận ca";
                }
                else
                {
                    var job1 = db.JOBS.Where(j => j.Sta == "1" && j.Id_job != id_job && j.Ngay == job.Ngay && j.Id_user == job.Id_user &&
                                           ((job.Tend > j.Tstart && j.Tend > job.Tstart) ||
                                             (job.Tstart >= j.Tstart && job.Tstart < j.Tend) ||
                                                (job.Tend > j.Tstart && job.Tend <= j.Tend)));
                    if (!job1.Any())
                    {
                        job.Id_user = id_user;
                        job.Status_change = "";
                        db.SaveChanges();
                        TempData["ErrorChuyenCa"] = "Nhận ca thành công";
                    }
                    else
                    {
                        TempData["ErrorChuyenCa"] = "Trùng lịch với những công việc khác";
                    }
                    db.SaveChanges();
                }
            }
            var job2 = db.JOBS.Where(a=>a.Id_user!=id_user && a.Status_change=="1" && a.Ngay>=currentDate).OrderBy(a=>a.Ngay).ToList();
            return View(job2);
        }
        public ActionResult NhuongCaAD(String id_job = "")
        {
            if (!Security("0")) return RedirectToAction("login", "Login");
            DateTime currentDate = DateTime.Now.Date;
            
            var job2 = db.JOBS.Where(a => a.Status_change == "1" && a.Ngay >= currentDate).OrderBy(a => a.Ngay).ToList();
            return View(job2);
        }
        public ActionResult NhanCaChiDinh()
        {
            if (!Security("1")) return RedirectToAction("login", "Login");
            string id_user = Session["Id"].ToString();
            var job = db.JOBS.Where(j => j.Id_change==id_user).ToList();
            return View(job);
        }

        public ActionResult Luong(string thang ="")
        {
            if (!Security("2")) return RedirectToAction("login", "Login");
            string id = Session["Id"].ToString();
            var luong = db.JOBS.Where(l => l.Id_hr == id && l.Sta == "1" && (l.Pay == "2" || l.Pay == "0")).OrderBy(l => l.Ngay).ToList();
            if (thang != "") luong = luong.Where(l => l.Ngay.Value.Month.ToString() == thang).ToList();
            return View(luong);
        }

        public ActionResult ConfirmLuong(string thang = "")
        {
            if (!Security("0")) return RedirectToAction("login", "Login");
            string id = Session["Id"].ToString();
            var luong = db.JOBS.Where(l => l.Id_hr == id && l.Sta == "1" && (l.Pay == "2" || l.Pay == "0")).OrderBy(l => l.Ngay).ToList();
            if (thang != "") luong = luong.Where(l => l.Ngay.Value.Month.ToString() == thang).ToList();
            return View(luong);
        }
    }
}
