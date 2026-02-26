using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Table;
using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Abstract.Table
{
    public interface ITableService
    {
        Task<SimpleResponse<Guid>> CreateTableAsync(string tableNumber, int capacity, string location);
        Task<SimpleResponse<string>> UpdateTableAsync(Guid id, string tableNumber, int capacity, string location);
        Task<SimpleResponse<string>> DeleteTableAsync(Guid id);
        Task<SimpleResponse<string>> ActivateTableAsync(Guid id);
        Task<SimpleResponse<string>> DeactivateTableAsync(Guid id);
        Task<SimpleResponse<string>> ChangeTableStatusAsync(Guid id, TableStatus status);
        Task<SimpleResponse<TableResponse>> GetByIdAsync(Guid id);
        Task<SimpleResponse<TableResponse>> GetByTableNumberAsync(string tableNumber);
        Task<SimpleResponse<List<TableSummaryResponse>>> GetAllAsync(bool? isActive = null, TableStatus? status = null);
        Task<SimpleResponse<List<TableSummaryResponse>>> GetAvailableTablesAsync();
        Task<SimpleResponse<List<TableSummaryResponse>>> GetOccupiedTablesAsync();
    }
}
