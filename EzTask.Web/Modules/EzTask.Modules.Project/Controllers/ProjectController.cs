using EzTask.Framework.Common;
using EzTask.Framework.Data;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Modules.Core.Controllers;
using EzTask.Modules.Project.ViewModels;
using EzTask.Web.Framework.Attributes;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EzTask.Modules.Project.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class ProjectController : BaseController
    {
        public ProjectController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        [Route("project.html")]
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalProject = await EzTask.Project.CountProjectByUser(Context.CurrentAccount.AccountId);
            return View();
        }

        #region Create project 

        /// <summary>
        /// Create project view
        /// </summary>
        /// <returns></returns>
        [Route("project/create-project.html")]
        public IActionResult CreateNew()
        {
            return View(new ProjectItemViewModel
            {
                ActionType = ActionType.CreateNew
            });
        }

        /// <summary>
        /// Create success project view
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        [Route("project/create-success.html")]
        public async Task<IActionResult> CreateSuccess(string code)
        {
            ProjectModel project = await EzTask.Project.GetProjectDetail(code);

            return View(project);
        }

        /// <summary>
        /// Create project action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("project/create-project.html")]
        public async Task<IActionResult> CreateNew(ProjectItemViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isDupplicated = await EzTask.Project.IsDupplicated(viewModel.ProjectName, viewModel.ProjectId);

                    if (isDupplicated)
                    {
                        ErrorMessage = Context.GetStringResource("ProjectIsDupplicated", StringResourceType.ProjectPage);
                    }
                    else
                    {
                        ProjectModel model = new ProjectModel
                        {
                            ProjectName = viewModel.ProjectName,
                            Description = viewModel.Description,
                            ProjectCode = string.Empty,
                            Status = ProjectStatus.Pending,
                            Owner = new AccountModel
                            {
                                AccountId = Context.CurrentAccount.AccountId
                            }
                        };

                        ProjectModel project = await EzTask.Project.Save(model);
                        if (project == null)
                        {
                            ErrorMessage = Context.GetStringResource("CreateProjectError", StringResourceType.ProjectPage);
                            viewModel.HasError = true;
                        }
                        else
                        {
                            SuccessMessage = Context.GetStringResource("CreateProjectSuccess", StringResourceType.ProjectPage);
                            return base.RedirectToAction("CreateSuccess",
                                new { code = project.ProjectCode });
                        }
                    }
                }
            }
            catch (Exception)
            {
                viewModel.HasError = true;
                ErrorMessage = Context.GetStringResource("CreateProjectError", StringResourceType.ProjectPage);
            }
            return View(viewModel);

        }
        #endregion

        #region Edit project

        /// <summary>
        /// Update project view
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        [Route("project/update-project.html")]
        public async Task<IActionResult> Update(string code)
        {
            var viewModel = new ProjectItemViewModel
            {
                ActionType = ActionType.Update
            };

            ProjectModel project = await EzTask.Project.GetProject(code);
            if (project == null)
            {
                return RedirectToAction("PageNotFound", "Common");
            }
            else
            {
                viewModel.Status = project.Status;
                viewModel.ProjectName = project.ProjectName;
                viewModel.Description = project.Description;
                viewModel.ProjectCode = project.ProjectCode;
                viewModel.ProjectId = project.ProjectId;
            }

            return View(viewModel);
        }

        /// <summary>
        /// update success project view
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("project/update-success.html")]
        public async Task<IActionResult> UpdateSuccess(string code)
        {
            ProjectModel project = await EzTask.Project.GetProjectDetail(code);
            return View(project);
        }

        /// <summary>
        /// Update project action
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("project/update-project.html")]
        public async Task<IActionResult> Update(ProjectItemViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProjectModel model = await EzTask.Project.GetProject(viewModel.ProjectCode);
                    if (model == null)
                    {
                        ErrorMessage = Context.GetStringResource("ErrorUpdateProject", StringResourceType.ProjectPage);
                        viewModel.HasError = true;
                    }
                    else
                    {
                        bool isDupplicated = await EzTask.Project.IsDupplicated(viewModel.ProjectName, model.ProjectId);

                        if (isDupplicated)
                        {
                            ErrorMessage = Context.GetStringResource("ProjectIsDupplicated", StringResourceType.ProjectPage);
                        }
                        else
                        {
                            model.ProjectName = viewModel.ProjectName;
                            model.Status = viewModel.Status;
                            model.Description = viewModel.Description;

                            ProjectModel project = await EzTask.Project.Save(model);
                            if (project == null)
                            {
                                ErrorMessage = Context.GetStringResource("ErrorUpdateProject", StringResourceType.ProjectPage);
                                model.HasError = true;
                            }
                            else
                            {
                                SuccessMessage = Context.GetStringResource("UpdateProjectSuccess", StringResourceType.ProjectPage);
                                return base.RedirectToAction("UpdateSuccess",
                                    new { code = project.ProjectCode });
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                viewModel.HasError = true;
                ErrorMessage = Context.GetStringResource("ProjectIsDupplicated", StringResourceType.ProjectPage);
            }
            return View(viewModel);

        }

        #endregion

        #region Other Action

        /// <summary>
        /// Remove project
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("project/remove.html")]
        [TokenAttribute]
        public async Task<IActionResult> RemoveProject(string code)
        {
            ResultModel<bool> iResult = await EzTask.Project.Delete(code);
            if (iResult.Status == ActionStatus.Ok)
            {
                return Ok();
            }
            else
            {
                return BadRequest(Context.GetStringResource("RequestFailed", StringResourceType.Error));
            }
        }

        /// <summary>
        /// Invite a specify person join to project
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("project/invite-new-member.html")]
        public async Task<IActionResult> InviteMember(int projectId, string accountName)
        {
            bool isNewMember = false;
            if (!Validator.IsValidEmailAddress(accountName))
            {
                return BadRequest(Context.GetStringResource("EmailWrongFormat", StringResourceType.Error));
            }

            AccountInfoModel account = await EzTask.Account.GetAccountInfo(accountName);

            if (account == null)
            {
                isNewMember = true;
                ResultModel<AccountModel> registerResult = await EzTask.Account.RegisterNew(new AccountModel
                {
                    AccountName = accountName,
                    Password = Guid.NewGuid().ToString().Substring(0, 6),
                    FullName = accountName,
                    DisplayName = accountName
                });

                if (registerResult.Status == ActionStatus.Ok)
                {
                    account = new AccountInfoModel { AccountId = registerResult.Data.AccountId };
                }
            }

            ProjectMemberModel model = new ProjectMemberModel
            {
                AccountId = account.AccountId,
                ProjectId = projectId,
                ActiveCode = Guid.NewGuid().ToString().Replace("-", "")
            };

            ResultModel<bool> alreadyAdded = await EzTask.Project.HasAlreadyAdded(model);

            if (alreadyAdded.Data)
            {
                return BadRequest(Context.GetStringResource("AddedAccount", StringResourceType.ProjectPage));
            }

            ResultModel<ProjectMemberModel> iResult = await EzTask.Project.AddMember(model);

            if (iResult.Status == ActionStatus.Ok)
            {
                string inviteTitle = Context.GetStringResource("InviteTitleEmail", StringResourceType.ProjectPage);
                await EzTask.Project.SendInvitation(model.ProjectId, model.AccountId,
                            isNewMember, inviteTitle, model.ActiveCode);
            }
            else
            {
                return BadRequest(Context.GetStringResource("RequestFailed", StringResourceType.Error));
            }

            return PartialView("_AddNewMemberItemTemplate", iResult.Data);
        }

        /// <summary>
        /// Accept action a specify person to join to project
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("project/accept-invite.html")]
        [AllowAnonymous]
        public async Task<IActionResult> AcceptInvite([FromQuery(Name = "ref")] string activeCode)
        {     
            if (string.IsNullOrWhiteSpace(activeCode))
            {
                return NotFound();
            }

            InviteResultViewModel model = null;

            var result = await EzTask.Project.AcceptInvitation(activeCode);

            if(result.Status == ActionStatus.Ok)
            {
                model = new InviteResultViewModel
                {
                    MemberName = result.Data.DisplayName
                };

                var projectInfo = await EzTask.Project.GetProjectByActiveCode(activeCode);
                model.ProjectName = projectInfo.Data.ProjectName;
                model.Manager = projectInfo.Data.Owner.DisplayName;
            }
            else
            {
                return RedirectToAction("Error", "Common");
            }

            return View(model);
        }

        #endregion

        #region Non action
        #endregion
    }
}
