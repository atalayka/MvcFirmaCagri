using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            var cagrilar = db.TblCagrilar.Where(x=>x.Durum == true).ToList();
            return View(cagrilar);
        }
        public ActionResult PasifCagrilar()
        {
            var cagrilar = db.TblCagrilar.Where(x => x.Durum == false).ToList();
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
            p.Durum = true;
            p.Tarih = DateTime.Today;
            p.CagriFirma = 4;
            db.TblCagrilar.Add(p);
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");

        }

        public ActionResult CagriDetay(int id)
        {
            var cagri = db.TblCagriDetay.Where(x=>x.Cagri==id).ToList();
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

    }
}