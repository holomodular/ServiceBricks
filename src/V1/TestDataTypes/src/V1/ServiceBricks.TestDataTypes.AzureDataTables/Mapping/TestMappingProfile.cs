using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.TestDataTypes.AzureDataTables
{
    /// <summary>
    /// This is an mapping profile for the Test domain object.
    /// </summary>
    public partial class TestMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<Test, TestDto>(
                (s, d) =>
                {
                    d.StorageKey = s.Key.ToString();
                    d.CreateDate = s.CreateDate;
                    d.UpdateDate = s.UpdateDate;
                    d.TestBool = s.TestBool;
                    d.TestBoolNull = s.TestBoolNull;
                    d.TestBoolNullWithValue = s.TestBoolNullWithValue;
                    d.TestBoolNullWithMixedValues = s.TestBoolNullWithMixedValues;
                   byte tempTestByte;
                    if (byte.TryParse(s.TestByte, out tempTestByte))
                        d.TestByte = tempTestByte;
                   byte tempTestByteNull;
                    if (byte.TryParse(s.TestByteNull, out tempTestByteNull))
                        d.TestByteNull = tempTestByteNull;
                   byte tempTestByteNullWithValue;
                    if (byte.TryParse(s.TestByteNullWithValue, out tempTestByteNullWithValue))
                        d.TestByteNullWithValue = tempTestByteNullWithValue;
                   byte tempTestByteNullWithMixedValues;
                    if (byte.TryParse(s.TestByteNullWithMixedValues, out tempTestByteNullWithMixedValues))
                        d.TestByteNullWithMixedValues = tempTestByteNullWithMixedValues;
                    d.TestByteArray = s.TestByteArray;
                    d.TestByteArrayNull = s.TestByteArrayNull;
                    d.TestByteArrayNullWithValue = s.TestByteArrayNullWithValue;
                    d.TestByteArrayNullWithMixedValues = s.TestByteArrayNullWithMixedValues;
                    d.TestShort = (short) s.TestShort;
                    d.TestShortNull = (short?) s.TestShortNull;
                    d.TestShortNullWithValue = (short?) s.TestShortNullWithValue;
                    d.TestShortNullWithMixedValues = (short?) s.TestShortNullWithMixedValues;
                    d.TestInt = s.TestInt;
                    d.TestIntNull = s.TestIntNull;
                    d.TestIntNullWithValue = s.TestIntNullWithValue;
                    d.TestIntNullWithMixedValues = s.TestIntNullWithMixedValues;
                    d.TestLong = s.TestLong;
                    d.TestLongNull = s.TestLongNull;
                    d.TestLongNullWithValue = s.TestLongNullWithValue;
                    d.TestLongNullWithMixedValues = s.TestLongNullWithMixedValues;
                    d.TestDateTime = s.TestDateTime;
                    d.TestDateTimeNull = s.TestDateTimeNull;
                    d.TestDateTimeNullWithValue = s.TestDateTimeNullWithValue;
                    d.TestDateTimeNullWithMixedValues = s.TestDateTimeNullWithMixedValues;
                    d.TestDateTimeOffset = s.TestDateTimeOffset;
                    d.TestDateTimeOffsetNull = s.TestDateTimeOffsetNull;
                    d.TestDateTimeOffsetNullWithValue = s.TestDateTimeOffsetNullWithValue;
                    d.TestDateTimeOffsetNullWithMixedValues = s.TestDateTimeOffsetNullWithMixedValues;
                    d.TestDateOnly = DateOnly.FromDateTime(s.TestDateOnly);
                    d.TestDateOnlyNull = s.TestDateOnlyNull.HasValue ? DateOnly.FromDateTime(s.TestDateOnlyNull.Value) : null;
                    d.TestDateOnlyNullWithValue = s.TestDateOnlyNullWithValue.HasValue ? DateOnly.FromDateTime(s.TestDateOnlyNullWithValue.Value) : null;
                    d.TestDateOnlyNullWithMixedValues = s.TestDateOnlyNullWithMixedValues.HasValue ? DateOnly.FromDateTime(s.TestDateOnlyNullWithMixedValues.Value) : null;
                    d.TestTimeOnly = TimeOnly.FromDateTime(s.TestTimeOnly);
                    d.TestTimeOnlyNull = s.TestTimeOnlyNull.HasValue ? TimeOnly.FromDateTime(s.TestTimeOnlyNull.Value) : null;
                    d.TestTimeOnlyNullWithValue = s.TestTimeOnlyNullWithValue.HasValue ? TimeOnly.FromDateTime(s.TestTimeOnlyNullWithValue.Value) : null;
                    d.TestTimeOnlyNullWithMixedValues = s.TestTimeOnlyNullWithMixedValues.HasValue ? TimeOnly.FromDateTime(s.TestTimeOnlyNullWithMixedValues.Value) : null;
                    d.TestDouble = s.TestDouble;
                    d.TestDoubleNull = s.TestDoubleNull;
                    d.TestDoubleNullWithValue = s.TestDoubleNullWithValue;
                    d.TestDoubleNullWithMixedValues = s.TestDoubleNullWithMixedValues;
                    d.TestDecimal = (decimal) s.TestDecimal;
                    d.TestDecimalNull = (decimal?) s.TestDecimalNull;
                    d.TestDecimalNullWithValue = (decimal?) s.TestDecimalNullWithValue;
                    d.TestDecimalNullWithMixedValues = (decimal?) s.TestDecimalNullWithMixedValues;
                    d.TestGuid = s.TestGuid;
                    d.TestGuidNull = s.TestGuidNull;
                    d.TestGuidNullWithValue = s.TestGuidNullWithValue;
                    d.TestGuidNullWithMixedValues = s.TestGuidNullWithMixedValues;
                    d.TestString = s.TestString;
                    d.TestStringNull = s.TestStringNull;
                    d.TestStringNullWithValue = s.TestStringNullWithValue;
                    d.TestStringNullWithMixedValues = s.TestStringNullWithMixedValues;
                    d.TestUShort = (ushort) s.TestUShort;
                    d.TestUShortNull = (ushort?) s.TestUShortNull;
                    d.TestUShortNullWithValue = (ushort?) s.TestUShortNullWithValue;
                    d.TestUShortNullWithMixedValues = (ushort?) s.TestUShortNullWithMixedValues;
                    d.TestUInt = (uint) s.TestUInt;
                    d.TestUIntNull = (uint?) s.TestUIntNull;
                    d.TestUIntNullWithValue = (uint?) s.TestUIntNullWithValue;
                    d.TestUIntNullWithMixedValues = (uint?) s.TestUIntNullWithMixedValues;
                   char tempTestChar;
                    if (char.TryParse(s.TestChar, out tempTestChar))
                        d.TestChar = tempTestChar;
                   char tempTestCharNull;
                    if (char.TryParse(s.TestCharNull, out tempTestCharNull))
                        d.TestCharNull = tempTestCharNull;
                   char tempTestCharNullWithValue;
                    if (char.TryParse(s.TestCharNullWithValue, out tempTestCharNullWithValue))
                        d.TestCharNullWithValue = tempTestCharNullWithValue;
                   char tempTestCharNullWithMixedValues;
                    if (char.TryParse(s.TestCharNullWithMixedValues, out tempTestCharNullWithMixedValues))
                        d.TestCharNullWithMixedValues = tempTestCharNullWithMixedValues;
                   TimeSpan tempTestTimeSpan;
                    if (TimeSpan.TryParse(s.TestTimeSpan, out tempTestTimeSpan))
                        d.TestTimeSpan = tempTestTimeSpan;
                   TimeSpan tempTestTimeSpanNull;
                    if (TimeSpan.TryParse(s.TestTimeSpanNull, out tempTestTimeSpanNull))
                        d.TestTimeSpanNull = tempTestTimeSpanNull;
                   TimeSpan tempTestTimeSpanNullWithValue;
                    if (TimeSpan.TryParse(s.TestTimeSpanNullWithValue, out tempTestTimeSpanNullWithValue))
                        d.TestTimeSpanNullWithValue = tempTestTimeSpanNullWithValue;
                   TimeSpan tempTestTimeSpanNullWithMixedValues;
                    if (TimeSpan.TryParse(s.TestTimeSpanNullWithMixedValues, out tempTestTimeSpanNullWithMixedValues))
                        d.TestTimeSpanNullWithMixedValues = tempTestTimeSpanNullWithMixedValues;
                    d.TestFloat = (float) s.TestFloat;
                    d.TestFloatNull = (float?) s.TestFloatNull;
                    d.TestFloatNullWithValue = (float?) s.TestFloatNullWithValue;
                    d.TestFloatNullWithMixedValues = (float?) s.TestFloatNullWithMixedValues;
                    d.TestSbyte = (sbyte) s.TestSbyte;
                    d.TestSbyteNull = (sbyte?) s.TestSbyteNull;
                    d.TestSbyteNullWithValue = (sbyte?) s.TestSbyteNullWithValue;
                    d.TestSbyteNullWithMixedValues = (sbyte?) s.TestSbyteNullWithMixedValues;

                });

            registry.Register<TestDto, Test>(
                (s, d) =>
                {
                   Guid tempStorageKey;
                    if (Guid.TryParse(s.StorageKey, out tempStorageKey))
                        d.Key = tempStorageKey;
                    //d.CreateDate ignored by rule
                    d.UpdateDate = s.UpdateDate;
                    d.TestBool = s.TestBool;
                    d.TestBoolNull = s.TestBoolNull;
                    d.TestBoolNullWithValue = s.TestBoolNullWithValue;
                    d.TestBoolNullWithMixedValues = s.TestBoolNullWithMixedValues;
                    d.TestByte = s.TestByte.ToString();
                    d.TestByteNull = s.TestByteNull.HasValue ? s.TestByteNull.Value.ToString() : null;
                    d.TestByteNullWithValue = s.TestByteNullWithValue.HasValue ? s.TestByteNullWithValue.Value.ToString() : null;
                    d.TestByteNullWithMixedValues = s.TestByteNullWithMixedValues.HasValue ? s.TestByteNullWithMixedValues.Value.ToString() : null;
                    d.TestByteArray = s.TestByteArray;
                    d.TestByteArrayNull = s.TestByteArrayNull;
                    d.TestByteArrayNullWithValue = s.TestByteArrayNullWithValue;
                    d.TestByteArrayNullWithMixedValues = s.TestByteArrayNullWithMixedValues;
                    d.TestShort = s.TestShort;
                    d.TestShortNull = s.TestShortNull;
                    d.TestShortNullWithValue = s.TestShortNullWithValue;
                    d.TestShortNullWithMixedValues = s.TestShortNullWithMixedValues;
                    d.TestInt = s.TestInt;
                    d.TestIntNull = s.TestIntNull;
                    d.TestIntNullWithValue = s.TestIntNullWithValue;
                    d.TestIntNullWithMixedValues = s.TestIntNullWithMixedValues;
                    d.TestLong = s.TestLong;
                    d.TestLongNull = s.TestLongNull;
                    d.TestLongNullWithValue = s.TestLongNullWithValue;
                    d.TestLongNullWithMixedValues = s.TestLongNullWithMixedValues;
                    d.TestDateTime = s.TestDateTime;
                    d.TestDateTimeNull = s.TestDateTimeNull;
                    d.TestDateTimeNullWithValue = s.TestDateTimeNullWithValue;
                    d.TestDateTimeNullWithMixedValues = s.TestDateTimeNullWithMixedValues;
                    d.TestDateTimeOffset = s.TestDateTimeOffset;
                    d.TestDateTimeOffsetNull = s.TestDateTimeOffsetNull;
                    d.TestDateTimeOffsetNullWithValue = s.TestDateTimeOffsetNullWithValue;
                    d.TestDateTimeOffsetNullWithMixedValues = s.TestDateTimeOffsetNullWithMixedValues;
                    d.TestDateOnly = s.TestDateOnly.ToDateTime(new TimeOnly(0, 0), DateTimeKind.Utc);
                    d.TestDateOnlyNull = s.TestDateOnlyNull.HasValue ? s.TestDateOnlyNull.Value.ToDateTime(new TimeOnly(0, 0), DateTimeKind.Utc) : null;
                    d.TestDateOnlyNullWithValue = s.TestDateOnlyNullWithValue.HasValue ? s.TestDateOnlyNullWithValue.Value.ToDateTime(new TimeOnly(0, 0), DateTimeKind.Utc) : null;
                    d.TestDateOnlyNullWithMixedValues = s.TestDateOnlyNullWithMixedValues.HasValue ? s.TestDateOnlyNullWithMixedValues.Value.ToDateTime(new TimeOnly(0, 0), DateTimeKind.Utc) : null;
                    d.TestTimeOnly = new DateTime(s.TestTimeOnly.Ticks, DateTimeKind.Utc).AddYears(StorageAzureDataTablesConstants.DATETIME_MINDATE.Year - 1);
                    d.TestTimeOnlyNull = s.TestTimeOnlyNull.HasValue ? new DateTime(s.TestTimeOnlyNull.Value.Ticks, DateTimeKind.Utc).AddYears(StorageAzureDataTablesConstants.DATETIME_MINDATE.Year - 1) : null;
                    d.TestTimeOnlyNullWithValue = s.TestTimeOnlyNullWithValue.HasValue ? new DateTime(s.TestTimeOnlyNullWithValue.Value.Ticks, DateTimeKind.Utc).AddYears(StorageAzureDataTablesConstants.DATETIME_MINDATE.Year - 1) : null;
                    d.TestTimeOnlyNullWithMixedValues = s.TestTimeOnlyNullWithMixedValues.HasValue ? new DateTime(s.TestTimeOnlyNullWithMixedValues.Value.Ticks, DateTimeKind.Utc).AddYears(StorageAzureDataTablesConstants.DATETIME_MINDATE.Year - 1) : null;
                    d.TestDouble = s.TestDouble;
                    d.TestDoubleNull = s.TestDoubleNull;
                    d.TestDoubleNullWithValue = s.TestDoubleNullWithValue;
                    d.TestDoubleNullWithMixedValues = s.TestDoubleNullWithMixedValues;
                    d.TestDecimal = (double) s.TestDecimal;
                    d.TestDecimalNull = (double?) s.TestDecimalNull;
                    d.TestDecimalNullWithValue = (double?) s.TestDecimalNullWithValue;
                    d.TestDecimalNullWithMixedValues = (double?) s.TestDecimalNullWithMixedValues;
                    d.TestGuid = s.TestGuid;
                    d.TestGuidNull = s.TestGuidNull;
                    d.TestGuidNullWithValue = s.TestGuidNullWithValue;
                    d.TestGuidNullWithMixedValues = s.TestGuidNullWithMixedValues;
                    d.TestString = s.TestString;
                    d.TestStringNull = s.TestStringNull;
                    d.TestStringNullWithValue = s.TestStringNullWithValue;
                    d.TestStringNullWithMixedValues = s.TestStringNullWithMixedValues;
                    d.TestUShort = s.TestUShort;
                    d.TestUShortNull = s.TestUShortNull;
                    d.TestUShortNullWithValue = s.TestUShortNullWithValue;
                    d.TestUShortNullWithMixedValues = s.TestUShortNullWithMixedValues;
                    d.TestUInt = s.TestUInt;
                    d.TestUIntNull = s.TestUIntNull;
                    d.TestUIntNullWithValue = s.TestUIntNullWithValue;
                    d.TestUIntNullWithMixedValues = s.TestUIntNullWithMixedValues;
                    d.TestChar = s.TestChar.ToString();
                    d.TestCharNull = s.TestCharNull.HasValue ? s.TestCharNull.Value.ToString() : null;
                    d.TestCharNullWithValue = s.TestCharNullWithValue.HasValue ? s.TestCharNullWithValue.Value.ToString() : null;
                    d.TestCharNullWithMixedValues = s.TestCharNullWithMixedValues.HasValue ? s.TestCharNullWithMixedValues.Value.ToString() : null;
                    d.TestTimeSpan = s.TestTimeSpan.ToString();
                    d.TestTimeSpanNull = s.TestTimeSpanNull.HasValue ? s.TestTimeSpanNull.Value.ToString() : null;
                    d.TestTimeSpanNullWithValue = s.TestTimeSpanNullWithValue.HasValue ? s.TestTimeSpanNullWithValue.Value.ToString() : null;
                    d.TestTimeSpanNullWithMixedValues = s.TestTimeSpanNullWithMixedValues.HasValue ? s.TestTimeSpanNullWithMixedValues.Value.ToString() : null;
                    d.TestFloat = s.TestFloat;
                    d.TestFloatNull = s.TestFloatNull;
                    d.TestFloatNullWithValue = s.TestFloatNullWithValue;
                    d.TestFloatNullWithMixedValues = s.TestFloatNullWithMixedValues;
                    d.TestSbyte = s.TestSbyte;
                    d.TestSbyteNull = s.TestSbyteNull;
                    d.TestSbyteNullWithValue = s.TestSbyteNullWithValue;
                    d.TestSbyteNullWithMixedValues = s.TestSbyteNullWithMixedValues;
                    d.PartitionKey = s.StorageKey;                    
                    //d.Etag ignored by rule
                    //d.Timestamp ignored by rule

                });
        }
    }
}
