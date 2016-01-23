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
    public class HighScoresController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/HighScores
        public IQueryable<HighScore> GetScores()
        {
            return db.Scores;
        }
        
        public IHttpActionResult GetTopScores(int? count)
        {
            if (count == null)
                count = 10;
            List<HighScore> scores = db.Scores.ToList();
            scores.OrderByDescending(sc => sc.Score);
            List<HighScore> topScores = new List<HighScore>();
            for (int i = 0; i < count; i++)
            {
                topScores.Add(scores[i]);
            }
            return Ok(topScores);
        }

        // GET: api/HighScores/5
        [ResponseType(typeof(HighScore))]
        public IHttpActionResult GetHighScore(int id)
        {
            HighScore highScore = db.Scores.Find(id);
            if (highScore == null)
            {
                return NotFound();
            }

            return Ok(highScore);
        }

        // PUT: api/HighScores/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHighScore(int id, HighScore highScore)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != highScore.ID)
            {
                return BadRequest();
            }

            db.Entry(highScore).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HighScoreExists(id))
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

        // POST: api/HighScores
        [ResponseType(typeof(HighScore))]
        public IHttpActionResult PostHighScore(HighScore highScore)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Scores.Add(highScore);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = highScore.ID }, highScore);
        }

        // DELETE: api/HighScores/5
        [ResponseType(typeof(HighScore))]
        public IHttpActionResult DeleteHighScore(int id)
        {
            HighScore highScore = db.Scores.Find(id);
            if (highScore == null)
            {
                return NotFound();
            }

            db.Scores.Remove(highScore);
            db.SaveChanges();

            return Ok(highScore);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HighScoreExists(int id)
        {
            return db.Scores.Count(e => e.ID == id) > 0;
        }
    }
}