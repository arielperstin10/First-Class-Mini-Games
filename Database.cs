﻿using System;
using System.IO;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace final_project
{
    public class Database : IDisposable
    {
        private string pathToExcel = @"..\..\Users.xlsx";

        public Database()
        {
            Console.WriteLine($"Excel file path: {Path.GetFullPath(pathToExcel)}");
            if (File.Exists(pathToExcel))
            {
                Console.WriteLine("Excel file exists.");
            }
            else
            {
                Console.WriteLine("File does not exist.");
                CreateExcelFile();
            }
        }

        private void OpenExcelFile(out XLWorkbook xlWorkbook, out IXLWorksheet xlWorksheet)
        {
            xlWorkbook = new XLWorkbook(Path.GetFullPath(pathToExcel));
            xlWorksheet = xlWorkbook.Worksheet("Users");
        }

        private void CreateExcelFile()
        {
            try
            {
                var xlWorkbook = new XLWorkbook();
                var xlWorksheet = xlWorkbook.Worksheets.Add("Users");

                xlWorksheet.Cell(1, 1).Value = "Username";
                xlWorksheet.Cell(1, 2).Value = "Password";
                xlWorksheet.Cell(1, 3).Value = "ID";
                xlWorksheet.Cell(1, 4).Value = "Email";
                xlWorksheet.Cell(1, 5).Value = "Gender";
                xlWorksheet.Cell(1, 6).Value = "Balance";

                xlWorkbook.Save();
                Console.WriteLine("Excel file created successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating Excel file: {ex.Message}");
            }
        }

        public int ValidateUser(string username, string password)
        {
            int isValidIndex = -1;

            OpenExcelFile(out XLWorkbook xlWorkbook, out IXLWorksheet xlWorksheet);

            try
            {
                var rows = xlWorksheet.RangeUsed().RowsUsed();
                foreach (var row in rows)
                {
                    if (row.RowNumber() == 1) continue; // Skip header row

                    if (row.Cell(1).GetValue<string>() == username &&
                        row.Cell(2).GetValue<string>() == password)
                    {
                        isValidIndex = row.RowNumber();
                        break;
                    }
                }
            }
            finally
            {
                xlWorkbook.Dispose();
            }

            return isValidIndex;
        }

        public bool RegisterUser(string username, string password, string id, string email, string gender)
        {
            bool isRegistered = false;

            OpenExcelFile(out XLWorkbook xlWorkbook, out IXLWorksheet xlWorksheet);

            try
            {
                var lastRow = xlWorksheet.LastRowUsed().RowNumber();
                var newRow = lastRow + 1;

                xlWorksheet.Cell(newRow, 1).Value = username;
                xlWorksheet.Cell(newRow, 2).Value = password;
                xlWorksheet.Cell(newRow, 3).Value = id;
                xlWorksheet.Cell(newRow, 4).Value = email;
                xlWorksheet.Cell(newRow, 5).Value = gender;
                xlWorksheet.Cell(newRow, 6).Value = 0;

                xlWorkbook.Save();
                isRegistered = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering user: {ex.Message}");
            }
            finally
            {
                xlWorkbook.Dispose();
            }

            return isRegistered;
        }

        public void SetBalance(int id, int wallet)
        {
            OpenExcelFile(out XLWorkbook xlWorkbook, out IXLWorksheet xlWorksheet);

            try
            {
                var rows = xlWorksheet.RangeUsed().RowsUsed();
                foreach (var row in rows)
                {
                    if (row.RowNumber() == 1) continue; // Skip header row

                    if (row.Cell(3).GetValue<string>() == id.ToString())
                    {
                        row.Cell(6).Value = wallet;
                        xlWorkbook.Save();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting balance: {ex.Message}");
            }
            finally
            {
                xlWorkbook.Dispose();
            }
        }

        public int GetBalance(int id)
        {
            int balance = -1;

            OpenExcelFile(out XLWorkbook xlWorkbook, out IXLWorksheet xlWorksheet);

            try
            {
                var rows = xlWorksheet.RangeUsed().RowsUsed();
                foreach (var row in rows)
                {
                    if (row.RowNumber() == 1) continue; // Skip header row

                    if (row.Cell(3).GetValue<string>() == id.ToString())
                    {
                        balance = row.Cell(6).GetValue<int>();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting balance: {ex.Message}");
            }
            finally
            {
                xlWorkbook.Dispose();
            }

            return balance;
        }

        public void SetUsername(int id, string newUsername)
        {
            OpenExcelFile(out XLWorkbook xlWorkbook, out IXLWorksheet xlWorksheet);

            try
            {
                var rows = xlWorksheet.RangeUsed().RowsUsed();
                foreach (var row in rows)
                {
                    if (row.RowNumber() == 1) continue; // Skip header row

                    if (row.Cell(3).GetValue<string>() == id.ToString())
                    {
                        row.Cell(1).Value = newUsername;
                        xlWorkbook.Save();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting username: {ex.Message}");
            }
            finally
            {
                xlWorkbook.Dispose();
            }
        }

        public void SetPassword(int id, string newPassword)
        {
            OpenExcelFile(out XLWorkbook xlWorkbook, out IXLWorksheet xlWorksheet);

            try
            {
                var rows = xlWorksheet.RangeUsed().RowsUsed();
                foreach (var row in rows)
                {
                    if (row.RowNumber() == 1) continue; // Skip header row

                    if (row.Cell(3).GetValue<string>() == id.ToString())
                    {
                        row.Cell(2).Value = newPassword;
                        xlWorkbook.Save();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting password: {ex.Message}");
            }
            finally
            {
                xlWorkbook.Dispose();
            }
        }

        public User LoadUserData(int index)
        {
            OpenExcelFile(out XLWorkbook xlWorkbook, out IXLWorksheet xlWorksheet);

            try
            {
                if (index < 2 || index > xlWorksheet.LastRowUsed().RowNumber())
                {
                    throw new ArgumentException("Invalid index");
                }

                var row = xlWorksheet.Row(index);
                var user = new User(
                    username: row.Cell(1).GetValue<string>(),
                    password: row.Cell(2).GetValue<string>(),
                    id: row.Cell(3).GetValue<string>(),
                    email: row.Cell(4).GetValue<string>(),
                    gender: row.Cell(5).GetValue<string>(),
                    balance: row.Cell(6).GetValue<int>()
                );

                if (string.IsNullOrEmpty(user.ID))
                {
                    throw new InvalidOperationException("User data not found");
                }

                return user;
            }
            finally
            {
                xlWorkbook.Dispose();
            }
        }

        public void Dispose()
        {
            // No action needed here, as each method handles its own cleanup
        }
    }
}
