using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceBricks.TestDataTypes.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class TestDataTypesV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreateDate = table.Column<byte[]>(type: "BLOB", nullable: false),
                    UpdateDate = table.Column<byte[]>(type: "BLOB", nullable: false),
                    TestBool = table.Column<bool>(type: "INTEGER", nullable: false),
                    TestBoolNull = table.Column<bool>(type: "INTEGER", nullable: true),
                    TestBoolNullWithValue = table.Column<bool>(type: "INTEGER", nullable: true),
                    TestBoolNullWithMixedValues = table.Column<bool>(type: "INTEGER", nullable: true),
                    TestByte = table.Column<byte>(type: "INTEGER", nullable: false),
                    TestByteNull = table.Column<byte>(type: "INTEGER", nullable: true),
                    TestByteNullWithValue = table.Column<byte>(type: "INTEGER", nullable: true),
                    TestByteNullWithMixedValues = table.Column<byte>(type: "INTEGER", nullable: true),
                    TestByteArray = table.Column<byte[]>(type: "BLOB", nullable: true),
                    TestByteArrayNull = table.Column<byte[]>(type: "BLOB", nullable: true),
                    TestByteArrayNullWithValue = table.Column<byte[]>(type: "BLOB", nullable: true),
                    TestByteArrayNullWithMixedValues = table.Column<byte[]>(type: "BLOB", nullable: true),
                    TestShort = table.Column<short>(type: "INTEGER", nullable: false),
                    TestShortNull = table.Column<short>(type: "INTEGER", nullable: true),
                    TestShortNullWithValue = table.Column<short>(type: "INTEGER", nullable: true),
                    TestShortNullWithMixedValues = table.Column<short>(type: "INTEGER", nullable: true),
                    TestInt = table.Column<int>(type: "INTEGER", nullable: false),
                    TestIntNull = table.Column<int>(type: "INTEGER", nullable: true),
                    TestIntNullWithValue = table.Column<int>(type: "INTEGER", nullable: true),
                    TestIntNullWithMixedValues = table.Column<int>(type: "INTEGER", nullable: true),
                    TestLong = table.Column<long>(type: "INTEGER", nullable: false),
                    TestLongNull = table.Column<long>(type: "INTEGER", nullable: true),
                    TestLongNullWithValue = table.Column<long>(type: "INTEGER", nullable: true),
                    TestLongNullWithMixedValues = table.Column<long>(type: "INTEGER", nullable: true),
                    TestDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TestDateTimeNull = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TestDateTimeNullWithValue = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TestDateTimeNullWithMixedValues = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TestDateTimeOffset = table.Column<byte[]>(type: "BLOB", nullable: false),
                    TestDateTimeOffsetNull = table.Column<byte[]>(type: "BLOB", nullable: true),
                    TestDateTimeOffsetNullWithValue = table.Column<byte[]>(type: "BLOB", nullable: true),
                    TestDateTimeOffsetNullWithMixedValues = table.Column<byte[]>(type: "BLOB", nullable: true),
                    TestDateOnly = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    TestDateOnlyNull = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    TestDateOnlyNullWithValue = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    TestDateOnlyNullWithMixedValues = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    TestTimeOnly = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    TestTimeOnlyNull = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    TestTimeOnlyNullWithValue = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    TestTimeOnlyNullWithMixedValues = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    TestDouble = table.Column<double>(type: "REAL", nullable: false),
                    TestDoubleNull = table.Column<double>(type: "REAL", nullable: true),
                    TestDoubleNullWithValue = table.Column<double>(type: "REAL", nullable: true),
                    TestDoubleNullWithMixedValues = table.Column<double>(type: "REAL", nullable: true),
                    TestDecimal = table.Column<decimal>(type: "TEXT", nullable: false),
                    TestDecimalNull = table.Column<decimal>(type: "TEXT", nullable: true),
                    TestDecimalNullWithValue = table.Column<decimal>(type: "TEXT", nullable: true),
                    TestDecimalNullWithMixedValues = table.Column<decimal>(type: "TEXT", nullable: true),
                    TestGuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    TestGuidNull = table.Column<Guid>(type: "TEXT", nullable: true),
                    TestGuidNullWithValue = table.Column<Guid>(type: "TEXT", nullable: true),
                    TestGuidNullWithMixedValues = table.Column<Guid>(type: "TEXT", nullable: true),
                    TestString = table.Column<string>(type: "TEXT", nullable: true),
                    TestStringNull = table.Column<string>(type: "TEXT", nullable: true),
                    TestStringNullWithValue = table.Column<string>(type: "TEXT", nullable: true),
                    TestStringNullWithMixedValues = table.Column<string>(type: "TEXT", nullable: true),
                    TestUShort = table.Column<ushort>(type: "INTEGER", nullable: false),
                    TestUShortNull = table.Column<ushort>(type: "INTEGER", nullable: true),
                    TestUShortNullWithValue = table.Column<ushort>(type: "INTEGER", nullable: true),
                    TestUShortNullWithMixedValues = table.Column<ushort>(type: "INTEGER", nullable: true),
                    TestUInt = table.Column<uint>(type: "INTEGER", nullable: false),
                    TestUIntNull = table.Column<uint>(type: "INTEGER", nullable: true),
                    TestUIntNullWithValue = table.Column<uint>(type: "INTEGER", nullable: true),
                    TestUIntNullWithMixedValues = table.Column<uint>(type: "INTEGER", nullable: true),
                    TestChar = table.Column<char>(type: "TEXT", nullable: false),
                    TestCharNull = table.Column<char>(type: "TEXT", nullable: true),
                    TestCharNullWithValue = table.Column<char>(type: "TEXT", nullable: true),
                    TestCharNullWithMixedValues = table.Column<char>(type: "TEXT", nullable: true),
                    TestTimeSpan = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    TestTimeSpanNull = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    TestTimeSpanNullWithValue = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    TestTimeSpanNullWithMixedValues = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    TestFloat = table.Column<float>(type: "REAL", nullable: false),
                    TestFloatNull = table.Column<float>(type: "REAL", nullable: true),
                    TestFloatNullWithValue = table.Column<float>(type: "REAL", nullable: true),
                    TestFloatNullWithMixedValues = table.Column<float>(type: "REAL", nullable: true),
                    TestSbyte = table.Column<sbyte>(type: "INTEGER", nullable: false),
                    TestSbyteNull = table.Column<sbyte>(type: "INTEGER", nullable: true),
                    TestSbyteNullWithValue = table.Column<sbyte>(type: "INTEGER", nullable: true),
                    TestSbyteNullWithMixedValues = table.Column<sbyte>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.Key);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Test");
        }
    }
}
