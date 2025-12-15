using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.TestDataTypes.EntityFrameworkCore
{
    /// <summary>
    /// The Test domain object.
    /// </summary>
    public partial class Test : EntityFrameworkCoreDomainObject<Test> , IDpCreateDate, IDpUpdateDate
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
        public byte TestByte { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public byte? TestByteNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public byte? TestByteNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public byte? TestByteNullWithMixedValues { get; set; }

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
        public short TestShort { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public short? TestShortNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public short? TestShortNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public short? TestShortNullWithMixedValues { get; set; }

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
        public DateOnly TestDateOnly { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateOnly? TestDateOnlyNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateOnly? TestDateOnlyNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public DateOnly? TestDateOnlyNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public TimeOnly TestTimeOnly { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public TimeOnly? TestTimeOnlyNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public TimeOnly? TestTimeOnlyNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public TimeOnly? TestTimeOnlyNullWithMixedValues { get; set; }

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
        public decimal TestDecimal { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public decimal? TestDecimalNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public decimal? TestDecimalNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public decimal? TestDecimalNullWithMixedValues { get; set; }

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
        public ushort TestUShort { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public ushort? TestUShortNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public ushort? TestUShortNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public ushort? TestUShortNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public uint TestUInt { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public uint? TestUIntNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public uint? TestUIntNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public uint? TestUIntNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public char TestChar { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public char? TestCharNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public char? TestCharNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public char? TestCharNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public TimeSpan TestTimeSpan { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public TimeSpan? TestTimeSpanNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public TimeSpan? TestTimeSpanNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public TimeSpan? TestTimeSpanNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public float TestFloat { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public float? TestFloatNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public float? TestFloatNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public float? TestFloatNullWithMixedValues { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public sbyte TestSbyte { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public sbyte? TestSbyteNull { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public sbyte? TestSbyteNullWithValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public sbyte? TestSbyteNullWithMixedValues { get; set; }


        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override Expression<Func<Test, bool>> DomainGetItemFilter(Test obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}
