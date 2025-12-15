using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceBricks.TestDataTypes.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class TestDataTypesV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TestDataTypes");

            migrationBuilder.CreateTable(
                name: "Test",
                schema: "TestDataTypes",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TestBool = table.Column<bool>(type: "boolean", nullable: false),
                    TestBoolNull = table.Column<bool>(type: "boolean", nullable: true),
                    TestBoolNullWithValue = table.Column<bool>(type: "boolean", nullable: true),
                    TestBoolNullWithMixedValues = table.Column<bool>(type: "boolean", nullable: true),
                    TestByte = table.Column<byte>(type: "smallint", nullable: false),
                    TestByteNull = table.Column<byte>(type: "smallint", nullable: true),
                    TestByteNullWithValue = table.Column<byte>(type: "smallint", nullable: true),
                    TestByteNullWithMixedValues = table.Column<byte>(type: "smallint", nullable: true),
                    TestByteArray = table.Column<byte[]>(type: "bytea", nullable: true),
                    TestByteArrayNull = table.Column<byte[]>(type: "bytea", nullable: true),
                    TestByteArrayNullWithValue = table.Column<byte[]>(type: "bytea", nullable: true),
                    TestByteArrayNullWithMixedValues = table.Column<byte[]>(type: "bytea", nullable: true),
                    TestShort = table.Column<short>(type: "smallint", nullable: false),
                    TestShortNull = table.Column<short>(type: "smallint", nullable: true),
                    TestShortNullWithValue = table.Column<short>(type: "smallint", nullable: true),
                    TestShortNullWithMixedValues = table.Column<short>(type: "smallint", nullable: true),
                    TestInt = table.Column<int>(type: "integer", nullable: false),
                    TestIntNull = table.Column<int>(type: "integer", nullable: true),
                    TestIntNullWithValue = table.Column<int>(type: "integer", nullable: true),
                    TestIntNullWithMixedValues = table.Column<int>(type: "integer", nullable: true),
                    TestLong = table.Column<long>(type: "bigint", nullable: false),
                    TestLongNull = table.Column<long>(type: "bigint", nullable: true),
                    TestLongNullWithValue = table.Column<long>(type: "bigint", nullable: true),
                    TestLongNullWithMixedValues = table.Column<long>(type: "bigint", nullable: true),
                    TestDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TestDateTimeNull = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TestDateTimeNullWithValue = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TestDateTimeNullWithMixedValues = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TestDateTimeOffset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TestDateTimeOffsetNull = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TestDateTimeOffsetNullWithValue = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TestDateTimeOffsetNullWithMixedValues = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TestDateOnly = table.Column<DateOnly>(type: "date", nullable: false),
                    TestDateOnlyNull = table.Column<DateOnly>(type: "date", nullable: true),
                    TestDateOnlyNullWithValue = table.Column<DateOnly>(type: "date", nullable: true),
                    TestDateOnlyNullWithMixedValues = table.Column<DateOnly>(type: "date", nullable: true),
                    TestTimeOnly = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    TestTimeOnlyNull = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    TestTimeOnlyNullWithValue = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    TestTimeOnlyNullWithMixedValues = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    TestDouble = table.Column<double>(type: "double precision", nullable: false),
                    TestDoubleNull = table.Column<double>(type: "double precision", nullable: true),
                    TestDoubleNullWithValue = table.Column<double>(type: "double precision", nullable: true),
                    TestDoubleNullWithMixedValues = table.Column<double>(type: "double precision", nullable: true),
                    TestDecimal = table.Column<decimal>(type: "numeric", nullable: false),
                    TestDecimalNull = table.Column<decimal>(type: "numeric", nullable: true),
                    TestDecimalNullWithValue = table.Column<decimal>(type: "numeric", nullable: true),
                    TestDecimalNullWithMixedValues = table.Column<decimal>(type: "numeric", nullable: true),
                    TestGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    TestGuidNull = table.Column<Guid>(type: "uuid", nullable: true),
                    TestGuidNullWithValue = table.Column<Guid>(type: "uuid", nullable: true),
                    TestGuidNullWithMixedValues = table.Column<Guid>(type: "uuid", nullable: true),
                    TestString = table.Column<string>(type: "text", nullable: true),
                    TestStringNull = table.Column<string>(type: "text", nullable: true),
                    TestStringNullWithValue = table.Column<string>(type: "text", nullable: true),
                    TestStringNullWithMixedValues = table.Column<string>(type: "text", nullable: true),
                    TestUShort = table.Column<int>(type: "integer", nullable: false),
                    TestUShortNull = table.Column<int>(type: "integer", nullable: true),
                    TestUShortNullWithValue = table.Column<int>(type: "integer", nullable: true),
                    TestUShortNullWithMixedValues = table.Column<int>(type: "integer", nullable: true),
                    TestUInt = table.Column<long>(type: "bigint", nullable: false),
                    TestUIntNull = table.Column<long>(type: "bigint", nullable: true),
                    TestUIntNullWithValue = table.Column<long>(type: "bigint", nullable: true),
                    TestUIntNullWithMixedValues = table.Column<long>(type: "bigint", nullable: true),
                    TestChar = table.Column<char>(type: "character(1)", nullable: false),
                    TestCharNull = table.Column<char>(type: "character(1)", nullable: true),
                    TestCharNullWithValue = table.Column<char>(type: "character(1)", nullable: true),
                    TestCharNullWithMixedValues = table.Column<char>(type: "character(1)", nullable: true),
                    TestTimeSpan = table.Column<long>(type: "bigint", nullable: false),
                    TestTimeSpanNull = table.Column<long>(type: "bigint", nullable: true),
                    TestTimeSpanNullWithValue = table.Column<long>(type: "bigint", nullable: true),
                    TestTimeSpanNullWithMixedValues = table.Column<long>(type: "bigint", nullable: true),
                    TestFloat = table.Column<float>(type: "real", nullable: false),
                    TestFloatNull = table.Column<float>(type: "real", nullable: true),
                    TestFloatNullWithValue = table.Column<float>(type: "real", nullable: true),
                    TestFloatNullWithMixedValues = table.Column<float>(type: "real", nullable: true),
                    TestSbyte = table.Column<short>(type: "smallint", nullable: false),
                    TestSbyteNull = table.Column<short>(type: "smallint", nullable: true),
                    TestSbyteNullWithValue = table.Column<short>(type: "smallint", nullable: true),
                    TestSbyteNullWithMixedValues = table.Column<short>(type: "smallint", nullable: true)
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
                name: "Test",
                schema: "TestDataTypes");
        }
    }
}
