﻿using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;
using TenmoServer.Security;
using System.Collections.Generic;
using System;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserDao userDao;

        public UserController(IUserDao _userDao)
        {
            this.userDao = _userDao;
        }

        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            List<User> allUsers = null;
            allUsers = userDao.GetUsers();

            if (allUsers == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(allUsers);
            }
        }

        [HttpGet("/user/id/{userId}")]
        public ActionResult<User> GetUserById(int userId)
        {
            User returnUser = null;
            returnUser = userDao.GetUserById(userId);

            if (returnUser == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(returnUser);
            }
        }

        [HttpGet("/user/account/{accountId}")]

        public ActionResult<User> GetUserByAccountId(int accountId)
        {
            User returnUser = null;
            returnUser = userDao.GetUserByAccountId(accountId);

            if (returnUser == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(returnUser);
            }
        }



        [HttpGet("/user/name/{username}")]
        public ActionResult<User> GetUserByName(string username)
        {
            User returnUser = null;
            returnUser = userDao.GetUserByName(username);

            if (returnUser == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(returnUser);
            }
        }

        [HttpPost("register")]
        public ActionResult<User> AddUser(LoginUser userParam)
        {
            User newUser = new User();
            newUser = userDao.AddUser(userParam.Username, userParam.Password);

            if (newUser == null)
            {
                return StatusCode(406);
            }
            else
            {
                return Ok(newUser);
            }
        }
    }
}
