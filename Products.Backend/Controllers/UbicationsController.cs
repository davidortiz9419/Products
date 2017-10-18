namespace Products.Backend.Controllers
{
    using Domain;
    using Models;
    using System.Data.Entity;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    [Authorize(Users = "juandavidortiz07@hotmail.com")]
    public class UbicationsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Ubications
        public async Task<ActionResult> Index()
        {
            return View(await db.Ubications.ToListAsync());
        }

        // GET: Ubications/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ubication = await db.Ubications.FindAsync(id);

            if (ubication == null)
            {
                return HttpNotFound();
            }

            return View(ubication);
        }

        // GET: Ubications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ubications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Ubication ubication)
        {
            if (ModelState.IsValid)
            {
                db.Ubications.Add(ubication);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(ubication);
        }

        // GET: Ubications/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ubication = await db.Ubications.FindAsync(id);

            if (ubication == null)
            {
                return HttpNotFound();
            }

            return View(ubication);
        }

        // POST: Ubications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Ubication ubication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ubication).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(ubication);
        }

        // GET: Ubications/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ubication = await db.Ubications.FindAsync(id);

            if (ubication == null)
            {
                return HttpNotFound();
            }

            return View(ubication);
        }

        // POST: Ubications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var ubication = await db.Ubications.FindAsync(id);
            db.Ubications.Remove(ubication);
            await db.SaveChangesAsync();
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
    }
}
