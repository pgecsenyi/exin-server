// exin server
// Copyright (C) 2018  pgecsenyi
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Text;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExinServer.Data.Sqlite
{
    internal static class DatabaseContextExtensions
    {
        public static void SetSingleUnderscoreNameConvention(this ModelBuilder modelBuilder, bool doPreserveAcronyms)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                UpdateTableNames(entity, doPreserveAcronyms);
                UpdateColumnNames(entity, doPreserveAcronyms);
            }
        }

        private static void UpdateTableNames(IMutableEntityType entity, bool doPreserveAcronyms)
        {
            var tableName = entity.Relational().TableName;
            tableName = AddUndercoresToSentence(tableName, doPreserveAcronyms);
            tableName = tableName.ToLower();
            tableName = tableName.Singularize() ?? tableName;

            entity.Relational().TableName = tableName;
        }

        private static string AddUndercoresToSentence(string text, bool doPreserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            InsertUnderscores(newText, text, doPreserveAcronyms);

            return newText.ToString();
        }

        private static void InsertUnderscores(StringBuilder newText, string text, bool doPreserveAcronyms)
        {
            for (var i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                {
                    if (IsThePreviousLowerCase(text, i) || (doPreserveAcronyms && IsAtTheEndOfAcronym(text, i)))
                        newText.Append('_');
                }

                newText.Append(text[i]);
            }
        }

        private static bool IsThePreviousLowerCase(string text, int index)
        {
            var previous = text[index - 1];

            return previous != '_' && !char.IsUpper(previous);
        }

        private static bool IsAtTheEndOfAcronym(string text, int index)
        {
            return IsThisTheLastUppercase(text, index) && index < text.Length - 1;
        }

        private static bool IsThisTheLastUppercase(string text, int index)
        {
            var previous = text[index - 1];
            var next = text[index + 1];

            return char.IsUpper(previous) && !char.IsUpper(next);
        }

        private static void UpdateColumnNames(IMutableEntityType entity, bool doPreserveAcronyms)
        {
            foreach (var property in entity.GetProperties())
            {
                var columnName = property.Relational().ColumnName;
                columnName = AddUndercoresToSentence(columnName, doPreserveAcronyms);
                columnName = columnName.ToLower();

                property.Relational().ColumnName = columnName;
            }
        }
    }
}
