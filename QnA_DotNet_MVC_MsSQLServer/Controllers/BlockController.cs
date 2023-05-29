using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QnA_DotNet_MVC_MsSQLServer.Data;
using QnA_DotNet_MVC_MsSQLServer.Models;
using System.Security.Claims;

namespace QnA_DotNet_MVC_MsSQLServer.Controllers
{
    public class BlockController : Controller
    {
        private readonly QnA_DotNet_MVC_MsSQLServer_DBContext db = new QnA_DotNet_MVC_MsSQLServer_DBContext();

        public int GetUserId()
        {
            //var claimsId = (ClaimsIdentity)User.Identity;
            //var claims = claimsId.FindFirst(ClaimTypes.NameIdentifier);
            //string userID = claims.Value;

            //if (userID == null) userID = "empty";

            //return userID;

            //MORE FUNCTIONALITY IS NEEDED HERE TO ACTUALLY GET THER USER ID
            return 0;
        }

        // GET: Blocks
        public async Task<IActionResult> Index()
        {
            return View(await db.BlockTables.Where(x => x.IsAnswer.Equals(false)).ToListAsync());
        }

        // GET: Blocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.BlockTables == null)
            {
                return NotFound();
            }

            var block = await db.BlockTables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (block == null)
            {
                return NotFound();
            }

            List<BlockTable> answers_block_list = await db.BlockTables.Where(x =>
            x.IsAnswer.Equals(true) && x.IsAnswerToBlockId.Equals(id)).ToListAsync();
            this.ViewBag.answers_block_list = answers_block_list;
            this.ViewBag.contextUsers = db.UserTables;
            return View(block);
        }

        // GET: Blocks/Create
        public IActionResult Create()
        {
            return View("Create");
        }

        // POST: Blocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlockTitle,Text")] BlockTable block)
        {
            int userID = GetUserId();

            block.UserId = userID;

            block.IsAnswer = false;
            block.IsAnswerToBlockId = -1;
            block.DatePosted = DateTime.Now;

            //αsync 
            try
            {
                db.Add(block);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlockExists(block.IsAnswerToBlockId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Details", "Block", new { id = block.Id });


            return View(block);
        }

        // GET: Blocks/Answer
        public async Task<IActionResult> Answer(int? id)
        {
            if (id == null || db.BlockTables == null)
            {
                return NotFound();
            }

            var blockToAswer = await db.BlockTables.FirstOrDefaultAsync(m => m.Id == id);
            if (blockToAswer == null)
            {
                return NotFound();
            }

            //ViewBag can be used to pass data from controller to view, one way only in the same request.
            //this.TempData["blockToAswer"] = blockToAswer;
            this.ViewBag.blockToAswer = blockToAswer;
            //this.ViewData["blockToAswer"] = blockToAswer;

            return View();
        }

        // POST: Blocks/Answer
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Answer([Bind("Id,UserId,BlockTitle,Text")] BlockTable block,
            string IsAnswerToBlockId)
        {
            block.IsAnswer = true;
            block.IsAnswerToBlockId = int.Parse(IsAnswerToBlockId);
            block.DatePosted = DateTime.Now;
            block.Id = 0;

            int userID = GetUserId();

            block.UserId = userID;

            //αsync 
            try
            {
                db.Add(block);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlockExists(block.IsAnswerToBlockId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Details", "Block", new { id = block.IsAnswerToBlockId });
        }

        // GET: Blocks/Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == -1)
            {
                return RedirectToAction(nameof(Index));
            }

            var blockTable = db.BlockTables.Where(c => c.Id == id).ToList();
            BlockTable block = blockTable[0];
            return View("Edit", block);
        }

        // POST: Blocks/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,UserId,BlockTitle,Text")] BlockTable block)
        {

            block.DatePosted = DateTime.Now;


            var blockToUpdate = db.BlockTables.FirstOrDefault(t => t.Id == Id);

            if (blockToUpdate != null)
            {
                // Update the record
                blockToUpdate.Id = block.Id;
                blockToUpdate.UserId = block.UserId;
                blockToUpdate.BlockTitle = block.BlockTitle;
                blockToUpdate.Text = block.Text;
                blockToUpdate.DatePosted = block.DatePosted;

                // Submit the changes to the database
                //db.SaveChanges();

                //αsync 
                try
                {
                    db.Update(blockToUpdate);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlockExists(blockToUpdate.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View("Edit", blockToUpdate);
        }

        // GET: Blocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.BlockTables == null)
            {
                return NotFound();
            }

            var block = await db.BlockTables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (block == null)
            {
                return NotFound();
            }

            return View(block);
        }

        // POST: Blocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.BlockTables == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Block'  is null.");
            }
            var block = await db.BlockTables.FindAsync(id);

            if (block == null)
                return RedirectToAction(nameof(Index));

            if (block.UserId != GetUserId())
            {
                return Problem("You are not the one who created this block so you dont have permission to this");
            }

            db.BlockTables.Remove(block);

            await db.SaveChangesAsync();

            if (block.IsAnswerToBlockId != -1)
                return RedirectToAction("Details", "Block", new { id = block.IsAnswerToBlockId });
            else
                return RedirectToAction(nameof(Index));
        }

        private bool BlockExists(int id)
        {
            return db.BlockTables.Any(e => e.Id == id);
        }
    }
}
