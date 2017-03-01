using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace IndDev.Domain.Entity.TrackingUser
{
    public class Visitor
    {
        public int Id { get; set; }
        public string Identifer { get; set; }
        public string UserAgent { get; set; }
        public DateTime FirstDate { get; set; }
        public string StartUrl { get; set; }
        public string Email { get; set; }
        public DateTime LastVisit { get; set; }
        public virtual ICollection<UserRout> UserRouts { get; set; }

        public Visitor(){}

        public Visitor(HttpContextBase ctx)
        {
            UserAgent = ctx.Request.UserAgent;
            StartUrl = ctx.Request.Headers["Referer"];
            FirstDate = DateTime.Now;
            LastVisit = DateTime.Now;
        }
    }

    public class UserRout
    {
        public int Id { get; set; }
        public DateTime SetTime { get; set; }
        public string UrlString { get; set; }
        public string ControllerVisited { get; set; }
        public string ActionVisited { get; set; }
        public string UrlQuery { get; set; }
        public virtual Visitor Visitor { get; set; }

        public UserRout()
        {
            
        }

        public UserRout(HttpContextBase ctx)
        {
            SetTime = DateTime.Now;
            if (!string.IsNullOrEmpty(ctx.Request.Headers["Referer"]))
            {
                UrlString = ctx.Request.Headers["Referer"].ToString();
            }
            UrlQuery = ctx.Request.Url?.Query;
        }
    }
}