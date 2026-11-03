using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace QuanLyNhaThuoc.GUI
{
    // Xuất nội dung DataGridView ra file Excel (.xlsx) bằng thư viện ClosedXML.
    // Cài đặt: NuGet -> Install-Package ClosedXML
    // (project .NET Framework cũ hơn 4.6.2 thì cài bản: Install-Package ClosedXML -Version 0.95.4)
    // - Chỉ xuất các cột đang hiển thị, đúng thứ tự & thứ tự sắp xếp đang thấy trên lưới.
    // - Nếu lưới có cột ẩn "MucCanhBao": 2 = tô đỏ, 1 = tô vàng (giống trên form).
    public static class XuatExcel
    {
        private static readonly XLColor MAU_TIEU_DE_COT = XLColor.FromArgb(0, 122, 204);
        private static readonly XLColor DO_NEN = XLColor.FromArgb(255, 199, 206);
        private static readonly XLColor DO_CHU = XLColor.FromArgb(156, 0, 6);
        private static readonly XLColor VANG_NEN = XLColor.FromArgb(255, 235, 156);
        private static readonly XLColor VANG_CHU = XLColor.FromArgb(120, 85, 0);

        public static void TuDataGridView(DataGridView dgv, string duongDan, string tieuDe, string moTa, string tongKet)
        {
            List<DataGridViewColumn> cots = dgv.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible)
                .OrderBy(c => c.DisplayIndex)
                .ToList();
            if (cots.Count == 0) throw new InvalidOperationException("Không có cột nào để xuất.");

            int soCot = cots.Count;
            bool coMucCanhBao = dgv.Columns.Contains("MucCanhBao");

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add("BaoCao");
                int r = 1;

                // --- Tiêu đề ---
                ws.Cell(r, 1).SetValue((tieuDe ?? "BÁO CÁO").ToUpper());
                IXLRange rgTieuDe = ws.Range(r, 1, r, soCot);
                rgTieuDe.Merge();
                rgTieuDe.Style.Font.Bold = true;
                rgTieuDe.Style.Font.FontSize = 14;
                rgTieuDe.Style.Font.FontColor = MAU_TIEU_DE_COT;
                rgTieuDe.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                r++;

                // --- Điều kiện lọc + ngày xuất ---
                if (!string.IsNullOrEmpty(moTa))
                {
                    ws.Cell(r, 1).SetValue(moTa);
                    IXLRange rg = ws.Range(r, 1, r, soCot);
                    rg.Merge();
                    rg.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rg.Style.Font.Italic = true;
                    r++;
                }
                ws.Cell(r, 1).SetValue("Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                IXLRange rgNgay = ws.Range(r, 1, r, soCot);
                rgNgay.Merge();
                rgNgay.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rgNgay.Style.Font.FontColor = XLColor.Gray;
                r += 2;

                // --- Dòng tiêu đề cột ---
                int dongTieuDeCot = r;
                for (int c = 0; c < soCot; c++)
                    ws.Cell(r, c + 1).SetValue(cots[c].HeaderText);

                IXLRange rgHeader = ws.Range(r, 1, r, soCot);
                rgHeader.Style.Font.Bold = true;
                rgHeader.Style.Font.FontColor = XLColor.White;
                rgHeader.Style.Fill.BackgroundColor = MAU_TIEU_DE_COT;
                rgHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rgHeader.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                rgHeader.Style.Alignment.WrapText = true;
                r++;

                // --- Dữ liệu ---
                int dongDauData = r;
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.IsNewRow) continue;

                    for (int c = 0; c < soCot; c++)
                        GhiGiaTri(ws.Cell(r, c + 1), row.Cells[cots[c].Index].Value, cots[c].DefaultCellStyle.Format);

                    if (coMucCanhBao)
                    {
                        object v = row.Cells["MucCanhBao"].Value;
                        int muc = (v == null || v == DBNull.Value) ? 0 : Convert.ToInt32(v);
                        if (muc == 2 || muc == 1)
                        {
                            IXLRange rgDong = ws.Range(r, 1, r, soCot);
                            rgDong.Style.Fill.BackgroundColor = muc == 2 ? DO_NEN : VANG_NEN;
                            rgDong.Style.Font.FontColor = muc == 2 ? DO_CHU : VANG_CHU;
                        }
                    }
                    r++;
                }
                int dongCuoiData = Math.Max(r - 1, dongTieuDeCot);

                // Căn lề từng cột giống trên lưới
                if (dongCuoiData >= dongDauData)
                {
                    for (int c = 0; c < soCot; c++)
                    {
                        DataGridViewContentAlignment canLe = cots[c].DefaultCellStyle.Alignment;
                        IXLRange rgCot = ws.Range(dongDauData, c + 1, dongCuoiData, c + 1);
                        if (canLe == DataGridViewContentAlignment.MiddleCenter)
                            rgCot.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        else if (canLe == DataGridViewContentAlignment.MiddleRight)
                            rgCot.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    }
                }

                // Kẻ khung bảng
                IXLRange rgBang = ws.Range(dongTieuDeCot, 1, dongCuoiData, soCot);
                rgBang.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                rgBang.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // --- Dòng tổng kết ---
                if (!string.IsNullOrEmpty(tongKet))
                {
                    r = dongCuoiData + 2;
                    ws.Cell(r, 1).SetValue(tongKet.Trim());
                    IXLRange rgTong = ws.Range(r, 1, r, soCot);
                    rgTong.Merge();
                    rgTong.Style.Font.Bold = true;
                    rgTong.Style.Font.FontColor = XLColor.FromArgb(0, 70, 140);
                }

                // Độ rộng cột: vừa nội dung (chỉ tính từ dòng tiêu đề cột trở xuống)
                ws.Columns(1, soCot).AdjustToContents(dongTieuDeCot, dongCuoiData);
                for (int c = 1; c <= soCot; c++)
                {
                    IXLColumn col = ws.Column(c);
                    if (col.Width > 50) col.Width = 50;
                    if (col.Width < 8) col.Width = 8;
                }

                ws.SheetView.FreezeRows(dongTieuDeCot);
                ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                ws.PageSetup.FitToPages(1, 0);

                wb.SaveAs(duongDan);
            }
        }

        private static void GhiGiaTri(IXLCell cell, object v, string dinhDang)
        {
            if (v == null || v == DBNull.Value) return;

            if (v is DateTime)
            {
                cell.SetValue((DateTime)v);
                cell.Style.DateFormat.Format = (dinhDang != null && dinhDang.Contains("HH"))
                    ? "dd/mm/yyyy hh:mm"
                    : "dd/mm/yyyy";
            }
            else if (v is byte || v is short || v is int || v is long ||
                     v is decimal || v is double || v is float)
            {
                cell.SetValue(Convert.ToDouble(v));
                if (dinhDang == "N0") cell.Style.NumberFormat.Format = "#,##0";
            }
            else
            {
                cell.SetValue(v.ToString());
            }
        }
    }
}