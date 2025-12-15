using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.TestDataTypes.AzureDataTables
{
    /// <summary>
    /// The Test domain object.
    /// </summary>
    public partial class Test : AzureDataTablesDomainObject<Test> , IDpCreateDate, IDpUpdateDate
    {
            
        /// <summary>
        /// The entity storage key.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// create date
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// update date
        /// </summary>
        public DateTimeOffset UpdateDate { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public bool TestBool { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public bool? TestBoolNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public bool? TestBoolNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public bool? TestBoolNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string TestByte { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? TestByteNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? TestByteNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? TestByteNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public byte[] TestByteArray { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public byte[]? TestByteArrayNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public byte[]? TestByteArrayNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public byte[]? TestByteArrayNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int TestShort { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int? TestShortNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int? TestShortNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int? TestShortNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int TestInt { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int? TestIntNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int? TestIntNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int? TestIntNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public long TestLong { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public long? TestLongNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public long? TestLongNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public long? TestLongNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTime TestDateTime { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTime? TestDateTimeNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTime? TestDateTimeNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTime? TestDateTimeNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTimeOffset TestDateTimeOffset { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTimeOffset? TestDateTimeOffsetNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTimeOffset? TestDateTimeOffsetNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTimeOffset? TestDateTimeOffsetNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTime TestDateOnly { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTime? TestDateOnlyNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTime? TestDateOnlyNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTime? TestDateOnlyNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTime TestTimeOnly { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTime? TestTimeOnlyNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTime? TestTimeOnlyNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateTime? TestTimeOnlyNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public double TestDouble { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public double? TestDoubleNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public double? TestDoubleNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public double? TestDoubleNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public double TestDecimal { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public double? TestDecimalNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public double? TestDecimalNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public double? TestDecimalNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public Guid TestGuid { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public Guid? TestGuidNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public Guid? TestGuidNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public Guid? TestGuidNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string TestString { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? TestStringNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? TestStringNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? TestStringNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int TestUShort { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int? TestUShortNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int? TestUShortNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int? TestUShortNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public long TestUInt { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public long? TestUIntNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public long? TestUIntNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public long? TestUIntNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string TestChar { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? TestCharNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? TestCharNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? TestCharNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string TestTimeSpan { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? TestTimeSpanNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? TestTimeSpanNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? TestTimeSpanNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public double TestFloat { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public double? TestFloatNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public double? TestFloatNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public double? TestFloatNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int TestSbyte { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int? TestSbyteNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int? TestSbyteNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public int? TestSbyteNullWithMixedValues { get; set; }

    }
}
