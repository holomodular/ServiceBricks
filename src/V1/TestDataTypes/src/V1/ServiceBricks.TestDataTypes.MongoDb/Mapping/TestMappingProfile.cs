namespace ServiceBricks.TestDataTypes.MongoDb
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
                    d.TestByte = s.TestByte;
                    d.TestByteNull = s.TestByteNull;
                    d.TestByteNullWithValue = s.TestByteNullWithValue;
                    d.TestByteNullWithMixedValues = s.TestByteNullWithMixedValues;
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
                    d.TestDateOnly = s.TestDateOnly;
                    d.TestDateOnlyNull = s.TestDateOnlyNull;
                    d.TestDateOnlyNullWithValue = s.TestDateOnlyNullWithValue;
                    d.TestDateOnlyNullWithMixedValues = s.TestDateOnlyNullWithMixedValues;
                    d.TestTimeOnly = s.TestTimeOnly;
                    d.TestTimeOnlyNull = s.TestTimeOnlyNull;
                    d.TestTimeOnlyNullWithValue = s.TestTimeOnlyNullWithValue;
                    d.TestTimeOnlyNullWithMixedValues = s.TestTimeOnlyNullWithMixedValues;
                    d.TestDouble = s.TestDouble;
                    d.TestDoubleNull = s.TestDoubleNull;
                    d.TestDoubleNullWithValue = s.TestDoubleNullWithValue;
                    d.TestDoubleNullWithMixedValues = s.TestDoubleNullWithMixedValues;
                    d.TestDecimal = s.TestDecimal;
                    d.TestDecimalNull = s.TestDecimalNull;
                    d.TestDecimalNullWithValue = s.TestDecimalNullWithValue;
                    d.TestDecimalNullWithMixedValues = s.TestDecimalNullWithMixedValues;
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
                    d.TestChar = s.TestChar;
                    d.TestCharNull = s.TestCharNull;
                    d.TestCharNullWithValue = s.TestCharNullWithValue;
                    d.TestCharNullWithMixedValues = s.TestCharNullWithMixedValues;
                    d.TestTimeSpan = s.TestTimeSpan;
                    d.TestTimeSpanNull = s.TestTimeSpanNull;
                    d.TestTimeSpanNullWithValue = s.TestTimeSpanNullWithValue;
                    d.TestTimeSpanNullWithMixedValues = s.TestTimeSpanNullWithMixedValues;
                    d.TestFloat = s.TestFloat;
                    d.TestFloatNull = s.TestFloatNull;
                    d.TestFloatNullWithValue = s.TestFloatNullWithValue;
                    d.TestFloatNullWithMixedValues = s.TestFloatNullWithMixedValues;
                    d.TestSbyte = s.TestSbyte;
                    d.TestSbyteNull = s.TestSbyteNull;
                    d.TestSbyteNullWithValue = s.TestSbyteNullWithValue;
                    d.TestSbyteNullWithMixedValues = s.TestSbyteNullWithMixedValues;

                });

            registry.Register<TestDto, Test>(
                (s, d) =>
                {
                    d.Key = s.StorageKey;
                    //d.CreateDate ignored by rule
                    d.UpdateDate = s.UpdateDate;
                    d.TestBool = s.TestBool;
                    d.TestBoolNull = s.TestBoolNull;
                    d.TestBoolNullWithValue = s.TestBoolNullWithValue;
                    d.TestBoolNullWithMixedValues = s.TestBoolNullWithMixedValues;
                    d.TestByte = s.TestByte;
                    d.TestByteNull = s.TestByteNull;
                    d.TestByteNullWithValue = s.TestByteNullWithValue;
                    d.TestByteNullWithMixedValues = s.TestByteNullWithMixedValues;
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
                    d.TestDateOnly = s.TestDateOnly;
                    d.TestDateOnlyNull = s.TestDateOnlyNull;
                    d.TestDateOnlyNullWithValue = s.TestDateOnlyNullWithValue;
                    d.TestDateOnlyNullWithMixedValues = s.TestDateOnlyNullWithMixedValues;
                    d.TestTimeOnly = s.TestTimeOnly;
                    d.TestTimeOnlyNull = s.TestTimeOnlyNull;
                    d.TestTimeOnlyNullWithValue = s.TestTimeOnlyNullWithValue;
                    d.TestTimeOnlyNullWithMixedValues = s.TestTimeOnlyNullWithMixedValues;
                    d.TestDouble = s.TestDouble;
                    d.TestDoubleNull = s.TestDoubleNull;
                    d.TestDoubleNullWithValue = s.TestDoubleNullWithValue;
                    d.TestDoubleNullWithMixedValues = s.TestDoubleNullWithMixedValues;
                    d.TestDecimal = s.TestDecimal;
                    d.TestDecimalNull = s.TestDecimalNull;
                    d.TestDecimalNullWithValue = s.TestDecimalNullWithValue;
                    d.TestDecimalNullWithMixedValues = s.TestDecimalNullWithMixedValues;
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
                    d.TestChar = s.TestChar;
                    d.TestCharNull = s.TestCharNull;
                    d.TestCharNullWithValue = s.TestCharNullWithValue;
                    d.TestCharNullWithMixedValues = s.TestCharNullWithMixedValues;
                    d.TestTimeSpan = s.TestTimeSpan;
                    d.TestTimeSpanNull = s.TestTimeSpanNull;
                    d.TestTimeSpanNullWithValue = s.TestTimeSpanNullWithValue;
                    d.TestTimeSpanNullWithMixedValues = s.TestTimeSpanNullWithMixedValues;
                    d.TestFloat = s.TestFloat;
                    d.TestFloatNull = s.TestFloatNull;
                    d.TestFloatNullWithValue = s.TestFloatNullWithValue;
                    d.TestFloatNullWithMixedValues = s.TestFloatNullWithMixedValues;
                    d.TestSbyte = s.TestSbyte;
                    d.TestSbyteNull = s.TestSbyteNull;
                    d.TestSbyteNullWithValue = s.TestSbyteNullWithValue;
                    d.TestSbyteNullWithMixedValues = s.TestSbyteNullWithMixedValues;

                });
        }
    }
}
