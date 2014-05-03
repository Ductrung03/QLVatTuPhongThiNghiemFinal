using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;

namespace QLVatTuPhongThiNghiem.Repositories.Implements
{
    public class BaoCaoRepository : IBaoCaoRepository
    {
        private readonly AppDbContext _context;

        public BaoCaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ThongKeTheoPhongViewModel>> ThongKeThietBiTheoPhongAsync()
        {
            return await _context.ThongKeTheoPhongViewModel
                .FromSqlRaw("EXEC SP_ThongKeThietBiTheoPhong")
                .ToListAsync();
        }

        public async Task<IEnumerable<ThongKeSuDungTheoThangViewModel>> ThongKeSuDungTheoThangAsync(int nam)
        {
            return await _context.ThongKeSuDungTheoThangViewModel
                .FromSqlRaw("EXEC SP_ThongKeSuDungTheoThang @Nam",
                    new SqlParameter("@Nam", nam))
                .ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> BaoCaoChiPhiSuaChuaAsync(DateTime tuNgay, DateTime denNgay)
        {
            // For dynamic results, we need to use a different approach
            var connection = _context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = "EXEC SP_BaoCaoChiPhiSuaChua @TuNgay, @DenNgay";
            command.Parameters.Add(new SqlParameter("@TuNgay", tuNgay.Date));
            command.Parameters.Add(new SqlParameter("@DenNgay", denNgay.Date));

            var results = new List<dynamic>();

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var expando = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    expando[reader.GetName(i)] = reader[i];
                }
                results.Add(expando);
            }

            return results;
        }

        public async Task<IEnumerable<dynamic>> ThongKeDanhGiaCapDoAsync()
        {
            // For dynamic results, we need to use a different approach
            var connection = _context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = "EXEC SP_ThongKeDanhGiaCapDo";

            var results = new List<dynamic>();

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var expando = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    expando[reader.GetName(i)] = reader[i];
                }
                results.Add(expando);
            }

            return results;
        }
    }
}