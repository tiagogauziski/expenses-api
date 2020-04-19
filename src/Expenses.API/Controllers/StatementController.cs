﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expenses.API.ViewModel;
using Expenses.Application.Services.Statement;
using Expenses.Application.Services.Statement.ViewModel;
using Expenses.Domain.Core.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.API.Controllers
{
    [Route("statement")]
    [ApiController]
    public class StatementController : ApiController
    {
        private readonly IStatementService _statementService;

        public StatementController(
            IEventStore eventStore,
            IStatementService statementService)
        {
            _statementService = statementService;
        }

        /// <summary>
        /// Creates a statement
        /// </summary>
        /// <param name="model">Statement Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(SuccessfulResponse<StatementResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(
            [FromBody]CreateStatementRequest model)
        {
            var result = await _statementService.Create(model);
            if (result.Successful)
                return SuccessResponse(result);
            else
                return FailureResponse(result);
        }

        /// <summary>
        /// Update an statement
        /// </summary>
        /// <param name="statementId">Statement ID</param>
        /// <param name="model">Statement Model</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{statementId}")]
        [ProducesResponseType(typeof(SuccessfulResponse<StatementResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromRoute] Guid statementId,
            [FromBody]UpdateStatementRequest model)
        {
            model.Id = statementId;
            var result = await _statementService.Update(model);
            if (result.Successful)
                return SuccessResponse(result);
            else
                return FailureResponse(result);
        }

        /// <summary>
        /// Update an statement mount
        /// </summary>
        /// <param name="statementId">Statement ID</param>
        /// <param name="model">Statement Model</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{statementId}/amount")]
        [ProducesResponseType(typeof(SuccessfulResponse<StatementResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAmount(
            [FromRoute] Guid statementId,
            [FromBody]UpdateStatementAmountRequest model)
        {
            model.Id = statementId;
            var result = await _statementService.UpdateAmount(model);
            if (result.Successful)
                return SuccessResponse(result);
            else
                return FailureResponse(result);
        }

        /// <summary>
        /// Get Statement by ID
        /// </summary>
        /// <param name="statementId">Statement ID</param>
        /// <returns>Statement Model</returns>
        [HttpGet]
        [Route("{statementId}")]
        [ProducesResponseType(typeof(SuccessfulResponse<StatementResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromRoute] string statementId)
        {
            var result = await _statementService.GetById(statementId);
            if (result.Successful)
                return SuccessResponse(result);
            else
                return FailureResponse(result);
        }

        /// <summary>
        /// Get Statement List
        /// </summary>
        /// <param name="query">Get Statement List Query Parameters</param>
        /// <returns>Statement Model</returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(SuccessfulResponse<IEnumerable<StatementResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetList([FromQuery] GetStatementListRequest query)
        {
            var result = await _statementService.GetList(query);
            if (result.Successful)
                return SuccessResponse(result);
            else
                return FailureResponse(result);
        }

        /// <summary>
        /// Delete Statement
        /// </summary>
        /// <param name="statementId">Statement ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{statementId}")]
        [ProducesResponseType(typeof(SuccessfulResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string statementId)
        {
            var result = await _statementService.Delete(statementId);
            if (result.Successful)
                return SuccessResponse(result);
            else
                return FailureResponse(result);
        }
    }
}