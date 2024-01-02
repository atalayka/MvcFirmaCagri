using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using MvcFirmaCagri.Models.Entity;

namespace MvcFirmaCagri.Controllers
{

    [Authorize] //authentication indicator

    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        DbIsTakiipEntities db = new DbIsTakiipEntities();

        public ActionResult AktifCagrilar()
        {
            var mail = (string)Session["Mail"];
            ViewBag.m = mail;

            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();

            var cagrilar = db.TblCagrilar.Where(x => x.Durum == true && x.CagriFirma == id).ToList();

            return View(cagrilar);
        }
        public ActionResult PasifCagrilar()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();

            var cagrilar = db.TblCagrilar.Where(x => x.Durum == false && x.CagriFirma == id).ToList();
            return View(cagrilar);
        }

        [HttpGet]
        public ActionResult YeniCagri()
        {
            return View();
        }


        [HttpPost]
        public ActionResult YeniCagri(TblCagrilar p)
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();


            p.Durum = true;
            p.Tarih = DateTime.Today;
            p.CagriFirma = id;
            db.TblCagrilar.Add(p);
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");

        }

        public ActionResult CagriDetay(int id)
        {
            var cagri = db.TblCagriDetay.Where(x => x.Cagri == id).ToList();
            return View(cagri);
        }
        public ActionResult CagriGetir(int id)
        {
            var cagri = db.TblCagrilar.Find(id);
            return View("CagriGetir", cagri);
        }
        public ActionResult CagriDuzenle(TblCagrilar p)
        {
            var cagri = db.TblCagrilar.Find(p.ID);
            cagri.Konu = p.Konu;
            cagri.Aciklama = p.Aciklama;
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");
        }

        [HttpGet]
        public ActionResult ProfilDuzenle()
        {

            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();

            var profil = db.TblFirmalar.Where(x => x.ID == id).FirstOrDefault();
            return View(profil);
        }
        public ActionResult AnaSayfa()
        {

            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();

            var fotograf = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.Gorsel).FirstOrDefault();
            ViewBag.f1 = fotograf;

            var toplamCagri = db.TblCagrilar.Where(x => x.CagriFirma == id).Count();
            ViewBag.c1 = toplamCagri;


            var aktifCagri = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == true).Count();
            ViewBag.c2 = aktifCagri;

            var pasifCagri = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == false).Count();
            ViewBag.c3 = pasifCagri;

            var yetkili = db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Yetkili).FirstOrDefault();
            ViewBag.c4 = yetkili;

            var sektor = db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Sektör).FirstOrDefault();
            ViewBag.c5 = sektor;



            return View();
        }

        public PartialViewResult Partial1()
        {
            //true is unreaded message; false is readed message
            var mail = (string)Session["Mail"];
            var mesajlar = db.TblMesajlar.Where(x => x.Alici == mail && x.Durum == true).ToList();

            var mesajSayisi = db.TblMesajlar.Where(x => x.Alici == mail && x.Durum == true).Count();
            ViewBag.m1 = mesajSayisi;


            return PartialView(mesajlar);
        }

        public PartialViewResult Partial2()
        {

            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();

            var cagrilar = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == true).ToList();
            var cagrilarAktif = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == true).Count();
            ViewBag.ca1 = cagrilarAktif;

            var fotograf = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.Gorsel).FirstOrDefault();
            ViewBag.f1 = fotograf;

            return PartialView(cagrilar);
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); ;
            return RedirectToAction("Index", "Login");
        }

        public ActionResult GelenMesaj()
        {
            return View();
        }
        public ActionResult GonderilenMesaj()
        {
            return View();
        }
    }
}