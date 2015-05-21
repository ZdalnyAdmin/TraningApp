using AppEngine.Helpers;
using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.DataObject;
using AppEngine.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrganizationModule.Controllers
{
    public class GroupController : ApiController
    {
        private EFContext db = new EFContext();

        // POST api/<controller>
        [HttpPost]
        public HttpResponseMessage Post(GroupManagmentViewModel obj)
        {
            try
            {
                obj.ErrorMessage = String.Empty;
                if (obj.LoggedUser == null)
                {
                    obj.LoggedUser = Person.GetLoggedPerson(User);
                }

                if (obj.CurrentOrganization == null)
                {
                    obj.CurrentOrganization = db.Organizations.FirstOrDefault(x => x.OrganizationID == obj.LoggedUser.OrganizationID);
                }

                if (obj.CurrentOrganization == null)
                {
                    obj.ErrorMessage = "Brak organizacji do ktorej mozna przypisac grupe!";
                    return Request.CreateResponse(HttpStatusCode.Created, obj); ;
                }

                switch (obj.ActionType)
                {
                    case BaseActionType.Get:
                        //get groups assigned to organizaction
                        obj.Current = new ProfileGroup();


                        var groups = (from grp in db.GroupsInOrganizations
                                      join g in db.Groups on grp.ProfileGroupID equals g.ProfileGroupID
                                      where grp.OrganizationID == obj.CurrentOrganization.OrganizationID && g.Name != "Wszyscy" && !g.IsDeleted
                                      select new
                                      {
                                          ProfileGroupID = g.ProfileGroupID,
                                          Name = g.Name
                                      }
                         ).ToList();

                        obj.Groups = new List<ProfileGroup>();

                        if (groups.Any())
                        {
                            obj.Groups = (from grp in groups
                                          group grp by grp.ProfileGroupID
                                              into gp
                                              select new ProfileGroup
                                              {
                                                  ProfileGroupID = gp.Key,
                                                  Name = groups.FirstOrDefault(x => x.ProfileGroupID == gp.Key).Name
                                              }).ToList();
                        }


                        foreach (var g in obj.Groups)
                        {
                            g.AssignedPeople = (from pig in db.PeopleInGroups
                                                join p in db.Users on pig.PersonID equals p.Id
                                                where pig.ProfileGroupID == g.ProfileGroupID
                                                select p).ToList();
                        }

                        obj.Success = String.Empty;

                        break;
                    case BaseActionType.Delete:

                        obj.Current.IsDeleted = true;
                        obj.Current.DeletedUserID = obj.LoggedUser.Id;
                        obj.Current.DeletedDate = DateTime.Now;

                        var personsInGroups = (from t in db.PeopleInGroups
                                               where t.ProfileGroupID == obj.Current.ProfileGroupID
                                               select t).ToList();
                        if (personsInGroups != null && personsInGroups.Any())
                        {
                            db.PeopleInGroups.RemoveRange(personsInGroups);
                        }


                        db.Entry(obj).State = EntityState.Modified;

                        db.SaveChanges();

                        var current = obj.Groups.FirstOrDefault(x => x.ProfileGroupID == obj.Current.ProfileGroupID);
                        if (current != null)
                        {
                            obj.Groups.Remove(current);
                        }

                        obj.Success = "Dane usuniete!";

                        break;
                    case BaseActionType.Edit:

                        var group = db.Groups.FirstOrDefault(x => x.ProfileGroupID == obj.Current.ProfileGroupID);

                        group.Name = obj.Current.Name;

                        var toRemove = (from t in db.PeopleInGroups
                                        where t.ProfileGroupID == obj.Current.ProfileGroupID
                                        select t).ToList();
                        if (toRemove != null && toRemove.Any())
                        {
                            db.PeopleInGroups.RemoveRange(toRemove);
                        }

                        if (obj.Current.AssignedPeople != null && obj.Current.AssignedPeople.Any())
                        {
                            foreach (var item in obj.Current.AssignedPeople)
                            {
                                var pg = new ProfileGroup2Person();
                                pg.IsDeleted = false;
                                pg.PersonID = item.Id;
                                pg.ProfileGroupID = obj.Current.ProfileGroupID;
                                db.PeopleInGroups.Add(pg);
                            }
                        }

                        db.Entry(obj.Current).State = EntityState.Modified;

                        db.SaveChanges();

                        obj.Success = "Dane zapisane!";

                        break;

                    case BaseActionType.Add:


                        obj.Current.CreateDate = DateTime.Now;
                        obj.Current.CreateUserID = obj.LoggedUser.Id;
                        obj.Current.IsDeleted = false;

                        db.Groups.Add(obj.Current);

                        var gip = new ProfileGroup2Organization();
                        gip.OrganizationID = obj.CurrentOrganization.OrganizationID;
                        gip.ProfileGroupID = obj.Current.ProfileGroupID;

                        db.GroupsInOrganizations.Add(gip);

                        db.SaveChanges();

                        //assigned people to group
                        if (obj.Current.AssignedPeople != null)
                        {
                            foreach (var item in obj.Current.AssignedPeople)
                            {
                                var pg = new ProfileGroup2Person();
                                pg.IsDeleted = false;
                                pg.PersonID = item.Id;
                                pg.ProfileGroupID = obj.Current.ProfileGroupID;
                                db.PeopleInGroups.Add(pg);
                            }
                            db.SaveChanges();
                        }

                        obj.Groups.Add(obj.Current);
                        obj.Current = new ProfileGroup();

                        obj.Success = "Dane zapisane!";

                        break;
                    default:
                        break;
                }

                return Request.CreateResponse(HttpStatusCode.Created, obj); ;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }



        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
