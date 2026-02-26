using CloPosProject.Application.Abstract.Table;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Table;
using CloPosProject.Domain.Enums;
using CloPosProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Persistence.Concurate.Table
{
    public class TableService(ApplicationDbContext _context) : ITableService
    {
        public async Task<SimpleResponse<string>> ActivateTableAsync(Guid id)
        {
            var table = await _context.Tables.FindAsync(id);

            if (table == null)
                return new SimpleResponse<string>("Masa tapılmadı");

            table.Activate();
            await _context.SaveChangesAsync();

            return new SimpleResponse<string>("Masa aktivləşdirildi");
        }

        public async Task<SimpleResponse<string>> ChangeTableStatusAsync(Guid id, TableStatus status)
        {
            var table = await _context.Tables.FindAsync(id);

            if (table == null)
                return new SimpleResponse<string>("Masa tapılmadı");

            if (!table.IsActive)
                return new SimpleResponse<string>("Deaktiv masanın statusu dəyişdirilə bilməz");

            switch (status)
            {
                case TableStatus.Occupied:
                    table.MarkAsOccupied();
                    break;
                case TableStatus.Reserved:
                    table.MarkAsReserved();
                    break;
                case TableStatus.Available:
                    table.MarkAsAvailable();
                    break;
                case TableStatus.Cleaning:
                    table.MarkAsCleaning(); 
                    break;

            }
            await _context.SaveChangesAsync();

            return new SimpleResponse<string>($"Masa statusu dəyişdirildi: {status}");
        }

        public async Task<SimpleResponse<Guid>> CreateTableAsync(string tableNumber, int capacity, string location)
        {
            if (string.IsNullOrWhiteSpace(tableNumber))
                return new SimpleResponse<Guid>("Masa nömrəsi boş ola bilməz");

            if (capacity <= 0)
                return new SimpleResponse<Guid>("Tutum müsbət olmalıdır");

            var exists = await _context.Tables
                .AnyAsync(t => t.TableNumber.ToLower() == tableNumber.ToLower());

            if (exists)
                return new SimpleResponse<Guid>("Bu nömrəli masa artıq mövcuddur");

            var table = new CloPosProject.Domain.Entities.Table(
                tableNumber,
                capacity,
                location
            );

            await _context.Tables.AddAsync(table);
            await _context.SaveChangesAsync();

            return new SimpleResponse<Guid>("Masa uğurla yaradıldı", table.Id);
        }

        public async Task<SimpleResponse<string>> DeactivateTableAsync(Guid id)
        {
            var table = await _context.Tables.FindAsync(id);

            if (table == null)
                return new SimpleResponse<string>("Masa tapılmadı");

            if (table.Status == TableStatus.Occupied)
                return new SimpleResponse<string>("Məşğul masa deaktiv edilə bilməz");

            table.Deactivate();
            await _context.SaveChangesAsync();

            return new SimpleResponse<string>("Masa deaktivləşdirildi");
        }

        public async Task<SimpleResponse<string>> DeleteTableAsync(Guid id)
        {
            var table = await _context.Tables
                   .Include(t => t.Orders)
                   .FirstOrDefaultAsync(t => t.Id == id);

            if (table == null)
                return new SimpleResponse<string>("Masa tapılmadı");

            if (table.Status == TableStatus.Occupied)
                return new SimpleResponse<string>("Məşğul masa silinə bilməz");

            if (table.Orders.Any())
                return new SimpleResponse<string>("Bu masada sifariş tarixçəsi var, silinə bilməz. Deaktiv edin.");

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();

            return new SimpleResponse<string>("Masa uğurla silindi");
        }

        public async Task<SimpleResponse<List<TableSummaryResponse>>> GetAllAsync(bool? isActive = null, TableStatus? status = null)
        {
            var query = _context.Tables.AsQueryable();

            if (isActive.HasValue)
                query = query.Where(t => t.IsActive == isActive.Value);

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            var tables = await query
                .OrderBy(t => t.TableNumber)
                .ToListAsync();

            var responses = tables.Select(t => new TableSummaryResponse
            {
                Id = t.Id,
                TableNumber = t.TableNumber,
                Capacity = t.Capacity,
                Status = t.Status,
                Location = t.Location,
                IsActive = t.IsActive
            }).ToList();

            return new SimpleResponse<List<TableSummaryResponse>>(responses);
        }

        public async Task<SimpleResponse<List<TableSummaryResponse>>> GetAvailableTablesAsync()
        {
            return await GetAllAsync(isActive: true, status: TableStatus.Available);
        }

        public async Task<SimpleResponse<TableResponse>> GetByIdAsync(Guid id)
        {
            var table = await _context.Tables
                   .Include(t => t.Orders.Where(o =>
                       o.Status == OrderStatus.Pending ||
                       o.Status == OrderStatus.Confirmed ||
                       o.Status == OrderStatus.Preparing ||
                       o.Status == OrderStatus.Ready))
                   .FirstOrDefaultAsync(t => t.Id == id);

            if (table == null)
                return new SimpleResponse<TableResponse>("Masa tapılmadı");

            var activeOrders = table.Orders
                .Where(o => o.Status != OrderStatus.Completed && o.Status != OrderStatus.Cancelled)
                .ToList();

            var currentBill = activeOrders.Sum(o => o.FinalAmount);

            var response = new TableResponse
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                Capacity = table.Capacity,
                Status = table.Status,
                IsActive = table.IsActive,
                Location = table.Location,
                ActiveOrdersCount = activeOrders.Count,
                CurrentBill = currentBill
            };

            return new SimpleResponse<TableResponse>(response);
        }

        public async Task<SimpleResponse<TableResponse>> GetByTableNumberAsync(string tableNumber)
        {
            var table = await _context.Tables
                    .Include(t => t.Orders.Where(o =>
                        o.Status == OrderStatus.Pending ||
                        o.Status == OrderStatus.Confirmed ||
                        o.Status == OrderStatus.Preparing ||
                        o.Status == OrderStatus.Ready))
                    .FirstOrDefaultAsync(t => t.TableNumber == tableNumber);

            if (table == null)
                return new SimpleResponse<TableResponse>("Masa tapılmadı");

            var activeOrders = table.Orders
                .Where(o => o.Status != OrderStatus.Completed && o.Status != OrderStatus.Cancelled)
                .ToList();

            var currentBill = activeOrders.Sum(o => o.FinalAmount);

            var response = new TableResponse
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                Capacity = table.Capacity,
                Status = table.Status,
                IsActive = table.IsActive,
                Location = table.Location,
                ActiveOrdersCount = activeOrders.Count,
                CurrentBill = currentBill
            };

            return new SimpleResponse<TableResponse>(response);
        }

        public async Task<SimpleResponse<List<TableSummaryResponse>>> GetOccupiedTablesAsync()
        {
            return await GetAllAsync(isActive: true, status: TableStatus.Occupied);
        }

        public async Task<SimpleResponse<string>> UpdateTableAsync(Guid id, string tableNumber, int capacity, string location)
        {
            var table = await _context.Tables.FindAsync(id);

            if (table == null)
                return new SimpleResponse<string>("Masa tapılmadı");

            if (string.IsNullOrWhiteSpace(tableNumber))
                return new SimpleResponse<string>("Masa nömrəsi boş ola bilməz");

            if (capacity <= 0)
                return new SimpleResponse<string>("Tutum müsbət olmalıdır");

            var duplicateExists = await _context.Tables
                .AnyAsync(t => t.Id != id && t.TableNumber.ToLower() == tableNumber.ToLower());

            if (duplicateExists)
                return new SimpleResponse<string>("Bu nömrəli başqa masa mövcuddur");

            table.UpdateDetails(tableNumber, capacity, location);
            await _context.SaveChangesAsync();

            return new SimpleResponse<string>("Masa uğurla yeniləndi");
        }
        private string GetStatusDisplay(TableStatus status)
        {
            return status switch
            {
                TableStatus.Available => "Boş",
                TableStatus.Occupied => "Məşğul",
                TableStatus.Reserved => "Rezerv",
                TableStatus.Cleaning => "Təmizlənir",
                _ => status.ToString()
            };
        }
    }
}
