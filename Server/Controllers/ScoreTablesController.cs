using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Server.Models;

namespace Server.Controllers
{
    public class ScoreTablesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ScoreTables
        public IQueryable<ScoreTable> GetScoreTables()
        {
            return db.ScoreTables;
        }

        // GET: api/ScoreTables/5
        [ResponseType(typeof(ScoreTable))]
        public IHttpActionResult GetScoreTable(int id)
        {
            ScoreTable scoreTable = db.ScoreTables.Find(id);
            if (scoreTable == null)
            {
                return NotFound();
            }

            return Ok(scoreTable);
        }

        // PUT: api/ScoreTables/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutScoreTable(int id, ScoreTable scoreTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != scoreTable.ID)
            {
                return BadRequest();
            }

            db.Entry(scoreTable).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScoreTableExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ScoreTables
        [ResponseType(typeof(ScoreTable))]
        public IHttpActionResult PostScoreTable(ScoreTable scoreTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ScoreTables.Add(scoreTable);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = scoreTable.ID }, scoreTable);
        }

        // DELETE: api/ScoreTables/5
        [ResponseType(typeof(ScoreTable))]
        public IHttpActionResult DeleteScoreTable(int id)
        {
            ScoreTable scoreTable = db.ScoreTables.Find(id);
            if (scoreTable == null)
            {
                return NotFound();
            }

            db.ScoreTables.Remove(scoreTable);
            db.SaveChanges();

            return Ok(scoreTable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ScoreTableExists(int id)
        {
            return db.ScoreTables.Count(e => e.ID == id) > 0;
        }
    }
}