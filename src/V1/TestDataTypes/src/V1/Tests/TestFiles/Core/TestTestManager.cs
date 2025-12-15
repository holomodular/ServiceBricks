using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.TestDataTypes;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public class TestTestManagerMongoDb : TestTestManager
    {
        public override TestDto GetObjectNotFound()
        {
            return new TestDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }

        public override void ValidateObjects(TestDto clientDto, TestDto serviceDto, HttpMethod method)
        {

            //PrimaryKeyRule
            if (method == HttpMethod.Post)
                Assert.True(!string.IsNullOrEmpty(serviceDto.StorageKey));
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);


            //CreateDateRule
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.CreateDate > clientDto.CreateDate);
            else
                Assert.True(serviceDto.CreateDate == clientDto.CreateDate);


            //UpdateDateRule
            if (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch)
                Assert.True(serviceDto.UpdateDate > clientDto.UpdateDate);
            else
                Assert.True(serviceDto.UpdateDate == clientDto.UpdateDate);


            Assert.True(serviceDto.TestBool == clientDto.TestBool);

            Assert.True(serviceDto.TestBoolNull == clientDto.TestBoolNull);

            Assert.True(serviceDto.TestBoolNullWithValue == clientDto.TestBoolNullWithValue);

            Assert.True(serviceDto.TestBoolNullWithMixedValues == clientDto.TestBoolNullWithMixedValues);

            Assert.True(serviceDto.TestByte == clientDto.TestByte);

            Assert.True(serviceDto.TestByteNull == clientDto.TestByteNull);

            Assert.True(serviceDto.TestByteNullWithValue == clientDto.TestByteNullWithValue);

            Assert.True(serviceDto.TestByteNullWithMixedValues == clientDto.TestByteNullWithMixedValues);

            Assert.True(serviceDto.TestByteArray != null && clientDto.TestByteArray != null ? serviceDto.TestByteArray.Length == clientDto.TestByteArray.Length : true);

            Assert.True(serviceDto.TestByteArrayNull != null && clientDto.TestByteArrayNull != null ? serviceDto.TestByteArrayNull.Length == clientDto.TestByteArrayNull.Length : true);

            Assert.True(serviceDto.TestByteArrayNullWithValue != null && clientDto.TestByteArrayNullWithValue != null ? serviceDto.TestByteArrayNullWithValue.Length == clientDto.TestByteArrayNullWithValue.Length : true);

            Assert.True(serviceDto.TestByteArrayNullWithMixedValues != null && clientDto.TestByteArrayNullWithMixedValues != null ? serviceDto.TestByteArrayNullWithMixedValues.Length == clientDto.TestByteArrayNullWithMixedValues.Length : true);

            Assert.True(serviceDto.TestShort == clientDto.TestShort);

            Assert.True(serviceDto.TestShortNull == clientDto.TestShortNull);

            Assert.True(serviceDto.TestShortNullWithValue == clientDto.TestShortNullWithValue);

            Assert.True(serviceDto.TestShortNullWithMixedValues == clientDto.TestShortNullWithMixedValues);

            Assert.True(serviceDto.TestInt == clientDto.TestInt);

            Assert.True(serviceDto.TestIntNull == clientDto.TestIntNull);

            Assert.True(serviceDto.TestIntNullWithValue == clientDto.TestIntNullWithValue);

            Assert.True(serviceDto.TestIntNullWithMixedValues == clientDto.TestIntNullWithMixedValues);

            Assert.True(serviceDto.TestLong == clientDto.TestLong);

            Assert.True(serviceDto.TestLongNull == clientDto.TestLongNull);

            Assert.True(serviceDto.TestLongNullWithValue == clientDto.TestLongNullWithValue);

            Assert.True(serviceDto.TestLongNullWithMixedValues == clientDto.TestLongNullWithMixedValues);
long datetimeTicks = 0;

                // Mongo special handling
                datetimeTicks = clientDto.TestDateTime.Ticks;
                datetimeTicks = ((long)(datetimeTicks / 10000)) * 10000;
                Assert.True(datetimeTicks == serviceDto.TestDateTime.Ticks);

            if (clientDto.TestDateTimeNull.HasValue && serviceDto.TestDateTimeNull.HasValue)
            {
                if (method == HttpMethod.Post)
                {
                    // MongoDb special handling
                    long ticks = clientDto.TestDateTimeNull.Value.Ticks;
                    ticks = ((long)(ticks / 10000)) * 10000;
                    Assert.True(ticks == serviceDto.TestDateTimeNull.Value.Ticks);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Patch)
                {
                    // MongoDb special handling
                    long ticks = clientDto.TestDateTimeNull.Value.Ticks;
                    ticks = ((long)(ticks / 10000)) * 10000;
                    Assert.True(ticks == serviceDto.TestDateTimeNull.Value.Ticks);
                }
                else if (method == HttpMethod.Get)
                {
                    // MongoDb special handling
                    long ticks = serviceDto.TestDateTimeNull.Value.Ticks;
                    ticks = ((long)(ticks / 10000)) * 10000;
                    Assert.True(ticks == clientDto.TestDateTimeNull.Value.Ticks);
                }
            }

            if (clientDto.TestDateTimeNullWithValue.HasValue && serviceDto.TestDateTimeNullWithValue.HasValue)
            {
                if (method == HttpMethod.Post)
                {
                    // MongoDb special handling
                    long ticks = clientDto.TestDateTimeNullWithValue.Value.Ticks;
                    ticks = ((long)(ticks / 10000)) * 10000;
                    Assert.True(ticks == serviceDto.TestDateTimeNullWithValue.Value.Ticks);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Patch)
                {
                    // MongoDb special handling
                    long ticks = clientDto.TestDateTimeNullWithValue.Value.Ticks;
                    ticks = ((long)(ticks / 10000)) * 10000;
                    Assert.True(ticks == serviceDto.TestDateTimeNullWithValue.Value.Ticks);
                }
                else if (method == HttpMethod.Get)
                {
                    // MongoDb special handling
                    long ticks = serviceDto.TestDateTimeNullWithValue.Value.Ticks;
                    ticks = ((long)(ticks / 10000)) * 10000;
                    Assert.True(ticks == clientDto.TestDateTimeNullWithValue.Value.Ticks);
                }
            }

            if (clientDto.TestDateTimeNullWithMixedValues.HasValue && serviceDto.TestDateTimeNullWithMixedValues.HasValue)
            {
                if (method == HttpMethod.Post)
                {
                    // MongoDb special handling
                    long ticks = clientDto.TestDateTimeNullWithMixedValues.Value.Ticks;
                    ticks = ((long)(ticks / 10000)) * 10000;
                    Assert.True(ticks == serviceDto.TestDateTimeNullWithMixedValues.Value.Ticks);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Patch)
                {
                    // MongoDb special handling
                    long ticks = clientDto.TestDateTimeNullWithMixedValues.Value.Ticks;
                    ticks = ((long)(ticks / 10000)) * 10000;
                    Assert.True(ticks == serviceDto.TestDateTimeNullWithMixedValues.Value.Ticks);
                }
                else if (method == HttpMethod.Get)
                {
                    // MongoDb special handling
                    long ticks = serviceDto.TestDateTimeNullWithMixedValues.Value.Ticks;
                    ticks = ((long)(ticks / 10000)) * 10000;
                    Assert.True(ticks == clientDto.TestDateTimeNullWithMixedValues.Value.Ticks);
                }
            }
long proputcTicks = 0;

            Assert.True(serviceDto.TestDateTimeOffset == clientDto.TestDateTimeOffset);

            Assert.True(serviceDto.TestDateTimeOffsetNull == clientDto.TestDateTimeOffsetNull);

            Assert.True(serviceDto.TestDateTimeOffsetNullWithValue == clientDto.TestDateTimeOffsetNullWithValue);

            Assert.True(serviceDto.TestDateTimeOffsetNullWithMixedValues == clientDto.TestDateTimeOffsetNullWithMixedValues);

            Assert.True(serviceDto.TestDateOnly == clientDto.TestDateOnly);

            Assert.True(serviceDto.TestDateOnlyNull == clientDto.TestDateOnlyNull);

            Assert.True(serviceDto.TestDateOnlyNullWithValue == clientDto.TestDateOnlyNullWithValue);

            Assert.True(serviceDto.TestDateOnlyNullWithMixedValues == clientDto.TestDateOnlyNullWithMixedValues);

            Assert.True(serviceDto.TestTimeOnly == clientDto.TestTimeOnly);

            Assert.True(serviceDto.TestTimeOnlyNull == clientDto.TestTimeOnlyNull);

            Assert.True(serviceDto.TestTimeOnlyNullWithValue == clientDto.TestTimeOnlyNullWithValue);

            Assert.True(serviceDto.TestTimeOnlyNullWithMixedValues == clientDto.TestTimeOnlyNullWithMixedValues);

            Assert.True(serviceDto.TestDouble == clientDto.TestDouble);

            Assert.True(serviceDto.TestDoubleNull == clientDto.TestDoubleNull);

            Assert.True(serviceDto.TestDoubleNullWithValue == clientDto.TestDoubleNullWithValue);

            Assert.True(serviceDto.TestDoubleNullWithMixedValues == clientDto.TestDoubleNullWithMixedValues);

            Assert.True(serviceDto.TestDecimal == clientDto.TestDecimal);

            Assert.True(serviceDto.TestDecimalNull == clientDto.TestDecimalNull);

            Assert.True(serviceDto.TestDecimalNullWithValue == clientDto.TestDecimalNullWithValue);

            Assert.True(serviceDto.TestDecimalNullWithMixedValues == clientDto.TestDecimalNullWithMixedValues);

            Assert.True(serviceDto.TestGuid == clientDto.TestGuid);

            Assert.True(serviceDto.TestGuidNull == clientDto.TestGuidNull);

            Assert.True(serviceDto.TestGuidNullWithValue == clientDto.TestGuidNullWithValue);

            Assert.True(serviceDto.TestGuidNullWithMixedValues == clientDto.TestGuidNullWithMixedValues);

            Assert.True(serviceDto.TestString == clientDto.TestString);

            Assert.True(serviceDto.TestStringNull == clientDto.TestStringNull);

            Assert.True(serviceDto.TestStringNullWithValue == clientDto.TestStringNullWithValue);

            Assert.True(serviceDto.TestStringNullWithMixedValues == clientDto.TestStringNullWithMixedValues);

            Assert.True(serviceDto.TestUShort == clientDto.TestUShort);

            Assert.True(serviceDto.TestUShortNull == clientDto.TestUShortNull);

            Assert.True(serviceDto.TestUShortNullWithValue == clientDto.TestUShortNullWithValue);

            Assert.True(serviceDto.TestUShortNullWithMixedValues == clientDto.TestUShortNullWithMixedValues);

            Assert.True(serviceDto.TestUInt == clientDto.TestUInt);

            Assert.True(serviceDto.TestUIntNull == clientDto.TestUIntNull);

            Assert.True(serviceDto.TestUIntNullWithValue == clientDto.TestUIntNullWithValue);

            Assert.True(serviceDto.TestUIntNullWithMixedValues == clientDto.TestUIntNullWithMixedValues);

            Assert.True(serviceDto.TestChar == clientDto.TestChar);

            Assert.True(serviceDto.TestCharNull == clientDto.TestCharNull);

            Assert.True(serviceDto.TestCharNullWithValue == clientDto.TestCharNullWithValue);

            Assert.True(serviceDto.TestCharNullWithMixedValues == clientDto.TestCharNullWithMixedValues);

            Assert.True(serviceDto.TestTimeSpan == clientDto.TestTimeSpan);

            Assert.True(serviceDto.TestTimeSpanNull == clientDto.TestTimeSpanNull);

            Assert.True(serviceDto.TestTimeSpanNullWithValue == clientDto.TestTimeSpanNullWithValue);

            Assert.True(serviceDto.TestTimeSpanNullWithMixedValues == clientDto.TestTimeSpanNullWithMixedValues);

            Assert.True(serviceDto.TestFloat == clientDto.TestFloat);

            Assert.True(serviceDto.TestFloatNull == clientDto.TestFloatNull);

            Assert.True(serviceDto.TestFloatNullWithValue == clientDto.TestFloatNullWithValue);

            Assert.True(serviceDto.TestFloatNullWithMixedValues == clientDto.TestFloatNullWithMixedValues);

            Assert.True(serviceDto.TestSbyte == clientDto.TestSbyte);

            Assert.True(serviceDto.TestSbyteNull == clientDto.TestSbyteNull);

            Assert.True(serviceDto.TestSbyteNullWithValue == clientDto.TestSbyteNullWithValue);

            Assert.True(serviceDto.TestSbyteNullWithMixedValues == clientDto.TestSbyteNullWithMixedValues);

        }
    }

    public class TestTestManagerPostgres : TestTestManager
    {
        public override void ValidateObjects(TestDto clientDto, TestDto serviceDto, HttpMethod method)
        {

            //PrimaryKeyRule
            if (method == HttpMethod.Post)
                Assert.True(!string.IsNullOrEmpty(serviceDto.StorageKey));
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);


            //CreateDateRule
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.CreateDate.UtcTicks >= ((long)(clientDto.CreateDate.UtcTicks / 10)) * 10);
            else if (method == HttpMethod.Get)
            {
                // Postgres special handling
                long utcTicks = serviceDto.CreateDate.UtcTicks;
                utcTicks = ((long)(utcTicks / 10)) * 10;
                Assert.True(utcTicks == clientDto.CreateDate.UtcTicks);
            }
            else
                Assert.True(serviceDto.CreateDate == clientDto.CreateDate);


            //UpdateDateRule
            if (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch)
                Assert.True(serviceDto.UpdateDate.UtcTicks >= ((long)(clientDto.CreateDate.UtcTicks / 10)) * 10);
            else
            {
                // Postgres special handling
                long utcTicks = serviceDto.UpdateDate.UtcTicks;
                utcTicks = ((long)(utcTicks / 10)) * 10;
                Assert.True(utcTicks == clientDto.UpdateDate.UtcTicks);
            }


            Assert.True(serviceDto.TestBool == clientDto.TestBool);

            Assert.True(serviceDto.TestBoolNull == clientDto.TestBoolNull);

            Assert.True(serviceDto.TestBoolNullWithValue == clientDto.TestBoolNullWithValue);

            Assert.True(serviceDto.TestBoolNullWithMixedValues == clientDto.TestBoolNullWithMixedValues);

            Assert.True(serviceDto.TestByte == clientDto.TestByte);

            Assert.True(serviceDto.TestByteNull == clientDto.TestByteNull);

            Assert.True(serviceDto.TestByteNullWithValue == clientDto.TestByteNullWithValue);

            Assert.True(serviceDto.TestByteNullWithMixedValues == clientDto.TestByteNullWithMixedValues);

            Assert.True(serviceDto.TestByteArray != null && clientDto.TestByteArray != null ? serviceDto.TestByteArray.Length == clientDto.TestByteArray.Length : true);

            Assert.True(serviceDto.TestByteArrayNull != null && clientDto.TestByteArrayNull != null ? serviceDto.TestByteArrayNull.Length == clientDto.TestByteArrayNull.Length : true);

            Assert.True(serviceDto.TestByteArrayNullWithValue != null && clientDto.TestByteArrayNullWithValue != null ? serviceDto.TestByteArrayNullWithValue.Length == clientDto.TestByteArrayNullWithValue.Length : true);

            Assert.True(serviceDto.TestByteArrayNullWithMixedValues != null && clientDto.TestByteArrayNullWithMixedValues != null ? serviceDto.TestByteArrayNullWithMixedValues.Length == clientDto.TestByteArrayNullWithMixedValues.Length : true);

            Assert.True(serviceDto.TestShort == clientDto.TestShort);

            Assert.True(serviceDto.TestShortNull == clientDto.TestShortNull);

            Assert.True(serviceDto.TestShortNullWithValue == clientDto.TestShortNullWithValue);

            Assert.True(serviceDto.TestShortNullWithMixedValues == clientDto.TestShortNullWithMixedValues);

            Assert.True(serviceDto.TestInt == clientDto.TestInt);

            Assert.True(serviceDto.TestIntNull == clientDto.TestIntNull);

            Assert.True(serviceDto.TestIntNullWithValue == clientDto.TestIntNullWithValue);

            Assert.True(serviceDto.TestIntNullWithMixedValues == clientDto.TestIntNullWithMixedValues);

            Assert.True(serviceDto.TestLong == clientDto.TestLong);

            Assert.True(serviceDto.TestLongNull == clientDto.TestLongNull);

            Assert.True(serviceDto.TestLongNullWithValue == clientDto.TestLongNullWithValue);

            Assert.True(serviceDto.TestLongNullWithMixedValues == clientDto.TestLongNullWithMixedValues);
long datetimeTicks = 0;

                if (method == HttpMethod.Post)
                {
                    // Postgres special handling
                    long ticks = clientDto.TestDateTime.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestDateTime.Ticks);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Patch)
                {
                    // Postgres special handling
                    long ticks = clientDto.TestDateTime.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestDateTime.Ticks);
                }
                else if (method == HttpMethod.Get)
                {
                    // Postgres special handling
                    long ticks = serviceDto.TestDateTime.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == clientDto.TestDateTime.Ticks);
                }

            if (clientDto.TestDateTimeNull.HasValue && serviceDto.TestDateTimeNull.HasValue)
            {
                if (method == HttpMethod.Post)
                {
                    // Postgres special handling
                    long ticks = clientDto.TestDateTimeNull.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestDateTimeNull.Value.Ticks);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Patch)
                {
                    // Postgres special handling
                    long ticks = clientDto.TestDateTimeNull.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestDateTimeNull.Value.Ticks);
                }
                else if (method == HttpMethod.Get)
                {
                    // Postgres special handling
                    long ticks = serviceDto.TestDateTimeNull.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == clientDto.TestDateTimeNull.Value.Ticks);
                }
            }

            if (clientDto.TestDateTimeNullWithValue.HasValue && serviceDto.TestDateTimeNullWithValue.HasValue)
            {
                if (method == HttpMethod.Post)
                {
                    // Postgres special handling
                    long ticks = clientDto.TestDateTimeNullWithValue.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestDateTimeNullWithValue.Value.Ticks);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Patch)
                {
                    // Postgres special handling
                    long ticks = clientDto.TestDateTimeNullWithValue.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestDateTimeNullWithValue.Value.Ticks);
                }
                else if (method == HttpMethod.Get)
                {
                    // Postgres special handling
                    long ticks = serviceDto.TestDateTimeNullWithValue.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == clientDto.TestDateTimeNullWithValue.Value.Ticks);
                }
            }

            if (clientDto.TestDateTimeNullWithMixedValues.HasValue && serviceDto.TestDateTimeNullWithMixedValues.HasValue)
            {
                if (method == HttpMethod.Post)
                {
                    // Postgres special handling
                    long ticks = clientDto.TestDateTimeNullWithMixedValues.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestDateTimeNullWithMixedValues.Value.Ticks);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Patch)
                {
                    // Postgres special handling
                    long ticks = clientDto.TestDateTimeNullWithMixedValues.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestDateTimeNullWithMixedValues.Value.Ticks);
                }
                else if (method == HttpMethod.Get)
                {
                    // Postgres special handling
                    long ticks = serviceDto.TestDateTimeNullWithMixedValues.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == clientDto.TestDateTimeNullWithMixedValues.Value.Ticks);
                }
            }
long proputcTicks = 0;

                // Postgres special handling
                if (method == HttpMethod.Post)
                {
                    // Postgres special handling
                    long utcTicks = clientDto.TestDateTimeOffset.UtcTicks;
                    utcTicks = ((long)(utcTicks / 10)) * 10;
                    Assert.True(serviceDto.TestDateTimeOffset.UtcTicks >= utcTicks);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Patch)
                {
                    // Postgres special handling
                    long utcTicks = clientDto.TestDateTimeOffset.UtcTicks;
                    utcTicks = ((long)(utcTicks / 10)) * 10;
                    Assert.True(utcTicks == serviceDto.TestDateTimeOffset.UtcTicks);
                }
                else if (method == HttpMethod.Get)
                {
                    // Postgres special handling
                    long utcTicks = serviceDto.TestDateTimeOffset.UtcTicks;
                    utcTicks = ((long)(utcTicks / 10)) * 10;
                    Assert.True(utcTicks == clientDto.TestDateTimeOffset.UtcTicks);
                }

            if (clientDto.TestDateTimeOffsetNull.HasValue && serviceDto.TestDateTimeOffsetNull.HasValue)
            {
                if (method == HttpMethod.Post)
                {
                    // Postgres special handling
                    long utcTicks = clientDto.TestDateTimeOffsetNull.Value.UtcTicks;
                    utcTicks = ((long)(utcTicks / 10)) * 10;
                    Assert.True(serviceDto.TestDateTimeOffsetNull.Value.UtcTicks >= utcTicks);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Patch)
                {
                    // Postgres special handling
                    long utcTicks = clientDto.TestDateTimeOffsetNull.Value.UtcTicks;
                    utcTicks = ((long)(utcTicks / 10)) * 10;
                    Assert.True(utcTicks == serviceDto.TestDateTimeOffsetNull.Value.UtcTicks);
                }
                else if (method == HttpMethod.Get)
                {
                    // Postgres special handling
                    long utcTicks = serviceDto.TestDateTimeOffsetNull.Value.UtcTicks;
                    utcTicks = ((long)(utcTicks / 10)) * 10;
                    Assert.True(utcTicks == clientDto.TestDateTimeOffsetNull.Value.UtcTicks);
                }
            }

            if (clientDto.TestDateTimeOffsetNullWithValue.HasValue && serviceDto.TestDateTimeOffsetNullWithValue.HasValue)
            {
                if (method == HttpMethod.Post)
                {
                    // Postgres special handling
                    long utcTicks = clientDto.TestDateTimeOffsetNullWithValue.Value.UtcTicks;
                    utcTicks = ((long)(utcTicks / 10)) * 10;
                    Assert.True(serviceDto.TestDateTimeOffsetNullWithValue.Value.UtcTicks >= utcTicks);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Patch)
                {
                    // Postgres special handling
                    long utcTicks = clientDto.TestDateTimeOffsetNullWithValue.Value.UtcTicks;
                    utcTicks = ((long)(utcTicks / 10)) * 10;
                    Assert.True(utcTicks == serviceDto.TestDateTimeOffsetNullWithValue.Value.UtcTicks);
                }
                else if (method == HttpMethod.Get)
                {
                    // Postgres special handling
                    long utcTicks = serviceDto.TestDateTimeOffsetNullWithValue.Value.UtcTicks;
                    utcTicks = ((long)(utcTicks / 10)) * 10;
                    Assert.True(utcTicks == clientDto.TestDateTimeOffsetNullWithValue.Value.UtcTicks);
                }
            }

            if (clientDto.TestDateTimeOffsetNullWithMixedValues.HasValue && serviceDto.TestDateTimeOffsetNullWithMixedValues.HasValue)
            {
                if (method == HttpMethod.Post)
                {
                    // Postgres special handling
                    long utcTicks = clientDto.TestDateTimeOffsetNullWithMixedValues.Value.UtcTicks;
                    utcTicks = ((long)(utcTicks / 10)) * 10;
                    Assert.True(serviceDto.TestDateTimeOffsetNullWithMixedValues.Value.UtcTicks >= utcTicks);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Patch)
                {
                    // Postgres special handling
                    long utcTicks = clientDto.TestDateTimeOffsetNullWithMixedValues.Value.UtcTicks;
                    utcTicks = ((long)(utcTicks / 10)) * 10;
                    Assert.True(utcTicks == serviceDto.TestDateTimeOffsetNullWithMixedValues.Value.UtcTicks);
                }
                else if (method == HttpMethod.Get)
                {
                    // Postgres special handling
                    long utcTicks = serviceDto.TestDateTimeOffsetNullWithMixedValues.Value.UtcTicks;
                    utcTicks = ((long)(utcTicks / 10)) * 10;
                    Assert.True(utcTicks == clientDto.TestDateTimeOffsetNullWithMixedValues.Value.UtcTicks);
                }
            }

            Assert.True(serviceDto.TestDateOnly == clientDto.TestDateOnly);

            Assert.True(serviceDto.TestDateOnlyNull == clientDto.TestDateOnlyNull);

            Assert.True(serviceDto.TestDateOnlyNullWithValue == clientDto.TestDateOnlyNullWithValue);

            Assert.True(serviceDto.TestDateOnlyNullWithMixedValues == clientDto.TestDateOnlyNullWithMixedValues);

                if (method == HttpMethod.Post)
                {
                    // Postgres special handling
                    long ticks = clientDto.TestTimeOnly.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestTimeOnly.Ticks);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Patch)
                {
                    // Postgres special handling
                    long ticks = clientDto.TestTimeOnly.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestTimeOnly.Ticks);
                }
                else if (method == HttpMethod.Get)
                {
                    // Postgres special handling
                    long ticks = serviceDto.TestTimeOnly.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == clientDto.TestTimeOnly.Ticks);
                }

            if (clientDto.TestTimeOnlyNull.HasValue && serviceDto.TestTimeOnlyNull.HasValue)
            {
                if (method == HttpMethod.Post)
                {
                    // MongoDb/Postgres special handling
                    long ticks = clientDto.TestTimeOnlyNull.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestTimeOnlyNull.Value.Ticks);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Patch)
                {
                    // MongoDb/Postgres special handling
                    long ticks = clientDto.TestTimeOnlyNull.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestTimeOnlyNull.Value.Ticks);
                }
                else if (method == HttpMethod.Get)
                {
                    // MongoDb/Postgres special handling
                    long ticks = serviceDto.TestTimeOnlyNull.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == clientDto.TestTimeOnlyNull.Value.Ticks);
                }
            }

            if (clientDto.TestTimeOnlyNullWithValue.HasValue && serviceDto.TestTimeOnlyNullWithValue.HasValue)
            {
                if (method == HttpMethod.Post)
                {
                    // MongoDb/Postgres special handling
                    long ticks = clientDto.TestTimeOnlyNullWithValue.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestTimeOnlyNullWithValue.Value.Ticks);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Patch)
                {
                    // MongoDb/Postgres special handling
                    long ticks = clientDto.TestTimeOnlyNullWithValue.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestTimeOnlyNullWithValue.Value.Ticks);
                }
                else if (method == HttpMethod.Get)
                {
                    // MongoDb/Postgres special handling
                    long ticks = serviceDto.TestTimeOnlyNullWithValue.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == clientDto.TestTimeOnlyNullWithValue.Value.Ticks);
                }
            }

            if (clientDto.TestTimeOnlyNullWithMixedValues.HasValue && serviceDto.TestTimeOnlyNullWithMixedValues.HasValue)
            {
                if (method == HttpMethod.Post)
                {
                    // MongoDb/Postgres special handling
                    long ticks = clientDto.TestTimeOnlyNullWithMixedValues.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestTimeOnlyNullWithMixedValues.Value.Ticks);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Patch)
                {
                    // MongoDb/Postgres special handling
                    long ticks = clientDto.TestTimeOnlyNullWithMixedValues.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == serviceDto.TestTimeOnlyNullWithMixedValues.Value.Ticks);
                }
                else if (method == HttpMethod.Get)
                {
                    // MongoDb/Postgres special handling
                    long ticks = serviceDto.TestTimeOnlyNullWithMixedValues.Value.Ticks;
                    ticks = ((long)(ticks / 10)) * 10;
                    Assert.True(ticks == clientDto.TestTimeOnlyNullWithMixedValues.Value.Ticks);
                }
            }

            Assert.True(serviceDto.TestDouble == clientDto.TestDouble);

            Assert.True(serviceDto.TestDoubleNull == clientDto.TestDoubleNull);

            Assert.True(serviceDto.TestDoubleNullWithValue == clientDto.TestDoubleNullWithValue);

            Assert.True(serviceDto.TestDoubleNullWithMixedValues == clientDto.TestDoubleNullWithMixedValues);

            Assert.True(serviceDto.TestDecimal == clientDto.TestDecimal);

            Assert.True(serviceDto.TestDecimalNull == clientDto.TestDecimalNull);

            Assert.True(serviceDto.TestDecimalNullWithValue == clientDto.TestDecimalNullWithValue);

            Assert.True(serviceDto.TestDecimalNullWithMixedValues == clientDto.TestDecimalNullWithMixedValues);

            Assert.True(serviceDto.TestGuid == clientDto.TestGuid);

            Assert.True(serviceDto.TestGuidNull == clientDto.TestGuidNull);

            Assert.True(serviceDto.TestGuidNullWithValue == clientDto.TestGuidNullWithValue);

            Assert.True(serviceDto.TestGuidNullWithMixedValues == clientDto.TestGuidNullWithMixedValues);

            Assert.True(serviceDto.TestString == clientDto.TestString);

            Assert.True(serviceDto.TestStringNull == clientDto.TestStringNull);

            Assert.True(serviceDto.TestStringNullWithValue == clientDto.TestStringNullWithValue);

            Assert.True(serviceDto.TestStringNullWithMixedValues == clientDto.TestStringNullWithMixedValues);

            Assert.True(serviceDto.TestUShort == clientDto.TestUShort);

            Assert.True(serviceDto.TestUShortNull == clientDto.TestUShortNull);

            Assert.True(serviceDto.TestUShortNullWithValue == clientDto.TestUShortNullWithValue);

            Assert.True(serviceDto.TestUShortNullWithMixedValues == clientDto.TestUShortNullWithMixedValues);

            Assert.True(serviceDto.TestUInt == clientDto.TestUInt);

            Assert.True(serviceDto.TestUIntNull == clientDto.TestUIntNull);

            Assert.True(serviceDto.TestUIntNullWithValue == clientDto.TestUIntNullWithValue);

            Assert.True(serviceDto.TestUIntNullWithMixedValues == clientDto.TestUIntNullWithMixedValues);

            if (clientDto.TestChar == '\0')
                Assert.True(serviceDto.TestChar == ' ');
            else
                Assert.True(serviceDto.TestChar == clientDto.TestChar);

            if (clientDto.TestCharNull == null || clientDto.TestCharNull == '\0')
                Assert.True(serviceDto.TestCharNull == ' ');
            else
                Assert.True(serviceDto.TestCharNull == clientDto.TestCharNull);

            if (clientDto.TestCharNullWithValue == null || clientDto.TestCharNullWithValue == '\0')
                Assert.True(serviceDto.TestCharNullWithValue == ' ');
            else
                Assert.True(serviceDto.TestCharNullWithValue == clientDto.TestCharNullWithValue);

            if (clientDto.TestCharNullWithMixedValues == null || clientDto.TestCharNullWithMixedValues == '\0')
                Assert.True(serviceDto.TestCharNullWithMixedValues == ' ');
            else
                Assert.True(serviceDto.TestCharNullWithMixedValues == clientDto.TestCharNullWithMixedValues);

            Assert.True(serviceDto.TestTimeSpan == clientDto.TestTimeSpan);

            Assert.True(serviceDto.TestTimeSpanNull == clientDto.TestTimeSpanNull);

            Assert.True(serviceDto.TestTimeSpanNullWithValue == clientDto.TestTimeSpanNullWithValue);

            Assert.True(serviceDto.TestTimeSpanNullWithMixedValues == clientDto.TestTimeSpanNullWithMixedValues);

            Assert.True(serviceDto.TestFloat == clientDto.TestFloat);

            Assert.True(serviceDto.TestFloatNull == clientDto.TestFloatNull);

            Assert.True(serviceDto.TestFloatNullWithValue == clientDto.TestFloatNullWithValue);

            Assert.True(serviceDto.TestFloatNullWithMixedValues == clientDto.TestFloatNullWithMixedValues);

            Assert.True(serviceDto.TestSbyte == clientDto.TestSbyte);

            Assert.True(serviceDto.TestSbyteNull == clientDto.TestSbyteNull);

            Assert.True(serviceDto.TestSbyteNullWithValue == clientDto.TestSbyteNullWithValue);

            Assert.True(serviceDto.TestSbyteNullWithMixedValues == clientDto.TestSbyteNullWithMixedValues);

        }
    }

    public class TestTestManagerAzureDataTables : TestTestManager
    {
        public override void ValidateObjects(TestDto clientDto, TestDto serviceDto, HttpMethod method)
        {
            DateTimeOffset dateTimeOffsetMinDate = new DateTimeOffset(1601, 1, 1, 0, 0, 0, TimeSpan.Zero);
            DateTime dateTimeMinDate = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateOnly dateOnlyMinDate = new DateOnly(1601, 1, 1);


            //PrimaryKeyRule
            if (method == HttpMethod.Post)
                Assert.True(!string.IsNullOrEmpty(serviceDto.StorageKey));
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);


            //CreateDateRule
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.CreateDate > clientDto.CreateDate);
            else
                Assert.True(serviceDto.CreateDate == clientDto.CreateDate);


            //UpdateDateRule
            if (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch)
                Assert.True(serviceDto.UpdateDate > clientDto.UpdateDate);
            else
                Assert.True(serviceDto.UpdateDate == clientDto.UpdateDate);


            Assert.True(serviceDto.TestBool == clientDto.TestBool);

            Assert.True(serviceDto.TestBoolNull == clientDto.TestBoolNull);

            Assert.True(serviceDto.TestBoolNullWithValue == clientDto.TestBoolNullWithValue);

            Assert.True(serviceDto.TestBoolNullWithMixedValues == clientDto.TestBoolNullWithMixedValues);

            Assert.True(serviceDto.TestByte == clientDto.TestByte);

            Assert.True(serviceDto.TestByteNull == clientDto.TestByteNull);

            Assert.True(serviceDto.TestByteNullWithValue == clientDto.TestByteNullWithValue);

            Assert.True(serviceDto.TestByteNullWithMixedValues == clientDto.TestByteNullWithMixedValues);

            Assert.True(serviceDto.TestByteArray != null && clientDto.TestByteArray != null ? serviceDto.TestByteArray.Length == clientDto.TestByteArray.Length : true);

            Assert.True(serviceDto.TestByteArrayNull != null && clientDto.TestByteArrayNull != null ? serviceDto.TestByteArrayNull.Length == clientDto.TestByteArrayNull.Length : true);

            Assert.True(serviceDto.TestByteArrayNullWithValue != null && clientDto.TestByteArrayNullWithValue != null ? serviceDto.TestByteArrayNullWithValue.Length == clientDto.TestByteArrayNullWithValue.Length : true);

            Assert.True(serviceDto.TestByteArrayNullWithMixedValues != null && clientDto.TestByteArrayNullWithMixedValues != null ? serviceDto.TestByteArrayNullWithMixedValues.Length == clientDto.TestByteArrayNullWithMixedValues.Length : true);

            Assert.True(serviceDto.TestShort == clientDto.TestShort);

            Assert.True(serviceDto.TestShortNull == clientDto.TestShortNull);

            Assert.True(serviceDto.TestShortNullWithValue == clientDto.TestShortNullWithValue);

            Assert.True(serviceDto.TestShortNullWithMixedValues == clientDto.TestShortNullWithMixedValues);

            Assert.True(serviceDto.TestInt == clientDto.TestInt);

            Assert.True(serviceDto.TestIntNull == clientDto.TestIntNull);

            Assert.True(serviceDto.TestIntNullWithValue == clientDto.TestIntNullWithValue);

            Assert.True(serviceDto.TestIntNullWithMixedValues == clientDto.TestIntNullWithMixedValues);

            Assert.True(serviceDto.TestLong == clientDto.TestLong);

            Assert.True(serviceDto.TestLongNull == clientDto.TestLongNull);

            Assert.True(serviceDto.TestLongNullWithValue == clientDto.TestLongNullWithValue);

            Assert.True(serviceDto.TestLongNullWithMixedValues == clientDto.TestLongNullWithMixedValues);

                // AzureDataTables special handling
                if (clientDto.TestDateTime < dateTimeMinDate)
                    Assert.True(serviceDto.TestDateTime == dateTimeMinDate);
                else
                    Assert.True(serviceDto.TestDateTime == clientDto.TestDateTime);

                // AzureDataTables special handling
                if (clientDto.TestDateTimeNull.HasValue && clientDto.TestDateTimeNull.Value < dateTimeMinDate)
                    Assert.True(serviceDto.TestDateTimeNull == dateTimeMinDate);
                else
                    Assert.True(serviceDto.TestDateTimeNull == clientDto.TestDateTimeNull);

                // AzureDataTables special handling
                if (clientDto.TestDateTimeNullWithValue.HasValue && clientDto.TestDateTimeNullWithValue.Value < dateTimeMinDate)
                    Assert.True(serviceDto.TestDateTimeNullWithValue == dateTimeMinDate);
                else
                    Assert.True(serviceDto.TestDateTimeNullWithValue == clientDto.TestDateTimeNullWithValue);

                // AzureDataTables special handling
                if (clientDto.TestDateTimeNullWithMixedValues.HasValue && clientDto.TestDateTimeNullWithMixedValues.Value < dateTimeMinDate)
                    Assert.True(serviceDto.TestDateTimeNullWithMixedValues == dateTimeMinDate);
                else
                    Assert.True(serviceDto.TestDateTimeNullWithMixedValues == clientDto.TestDateTimeNullWithMixedValues);

                // AzureDataTables special handling
                if (clientDto.TestDateTimeOffset < dateTimeMinDate)
                    Assert.True(serviceDto.TestDateTimeOffset == dateTimeMinDate);
                else
                    Assert.True(serviceDto.TestDateTimeOffset == clientDto.TestDateTimeOffset);

                // AzureDataTables special handling
                if (clientDto.TestDateTimeOffsetNull.HasValue && clientDto.TestDateTimeOffsetNull.Value < dateTimeOffsetMinDate)
                    Assert.True(serviceDto.TestDateTimeOffsetNull == dateTimeOffsetMinDate);
                else
                    Assert.True(serviceDto.TestDateTimeOffsetNull == clientDto.TestDateTimeOffsetNull);

                // AzureDataTables special handling
                if (clientDto.TestDateTimeOffsetNullWithValue.HasValue && clientDto.TestDateTimeOffsetNullWithValue.Value < dateTimeOffsetMinDate)
                    Assert.True(serviceDto.TestDateTimeOffsetNullWithValue == dateTimeOffsetMinDate);
                else
                    Assert.True(serviceDto.TestDateTimeOffsetNullWithValue == clientDto.TestDateTimeOffsetNullWithValue);

                // AzureDataTables special handling
                if (clientDto.TestDateTimeOffsetNullWithMixedValues.HasValue && clientDto.TestDateTimeOffsetNullWithMixedValues.Value < dateTimeOffsetMinDate)
                    Assert.True(serviceDto.TestDateTimeOffsetNullWithMixedValues == dateTimeOffsetMinDate);
                else
                    Assert.True(serviceDto.TestDateTimeOffsetNullWithMixedValues == clientDto.TestDateTimeOffsetNullWithMixedValues);

                // AzureDataTables special handling
                if (clientDto.TestDateOnly < dateOnlyMinDate)
                    Assert.True(serviceDto.TestDateOnly == dateOnlyMinDate);
                else
                    Assert.True(serviceDto.TestDateOnly == clientDto.TestDateOnly);

                // AzureDataTables special handling
                if (clientDto.TestDateOnlyNull.HasValue && clientDto.TestDateOnlyNull.Value < dateOnlyMinDate)
                    Assert.True(serviceDto.TestDateOnlyNull == dateOnlyMinDate);
                else
                    Assert.True(serviceDto.TestDateOnlyNull == clientDto.TestDateOnlyNull);

                // AzureDataTables special handling
                if (clientDto.TestDateOnlyNullWithValue.HasValue && clientDto.TestDateOnlyNullWithValue.Value < dateOnlyMinDate)
                    Assert.True(serviceDto.TestDateOnlyNullWithValue == dateOnlyMinDate);
                else
                    Assert.True(serviceDto.TestDateOnlyNullWithValue == clientDto.TestDateOnlyNullWithValue);

                // AzureDataTables special handling
                if (clientDto.TestDateOnlyNullWithMixedValues.HasValue && clientDto.TestDateOnlyNullWithMixedValues.Value < dateOnlyMinDate)
                    Assert.True(serviceDto.TestDateOnlyNullWithMixedValues == dateOnlyMinDate);
                else
                    Assert.True(serviceDto.TestDateOnlyNullWithMixedValues == clientDto.TestDateOnlyNullWithMixedValues);

            Assert.True(serviceDto.TestTimeOnly == clientDto.TestTimeOnly);

            Assert.True(serviceDto.TestTimeOnlyNull == clientDto.TestTimeOnlyNull);

            Assert.True(serviceDto.TestTimeOnlyNullWithValue == clientDto.TestTimeOnlyNullWithValue);

            Assert.True(serviceDto.TestTimeOnlyNullWithMixedValues == clientDto.TestTimeOnlyNullWithMixedValues);

            Assert.True(serviceDto.TestDouble == clientDto.TestDouble);

            Assert.True(serviceDto.TestDoubleNull == clientDto.TestDoubleNull);

            Assert.True(serviceDto.TestDoubleNullWithValue == clientDto.TestDoubleNullWithValue);

            Assert.True(serviceDto.TestDoubleNullWithMixedValues == clientDto.TestDoubleNullWithMixedValues);

            Assert.True(serviceDto.TestDecimal == clientDto.TestDecimal);

            Assert.True(serviceDto.TestDecimalNull == clientDto.TestDecimalNull);

            Assert.True(serviceDto.TestDecimalNullWithValue == clientDto.TestDecimalNullWithValue);

            Assert.True(serviceDto.TestDecimalNullWithMixedValues == clientDto.TestDecimalNullWithMixedValues);

            Assert.True(serviceDto.TestGuid == clientDto.TestGuid);

            Assert.True(serviceDto.TestGuidNull == clientDto.TestGuidNull);

            Assert.True(serviceDto.TestGuidNullWithValue == clientDto.TestGuidNullWithValue);

            Assert.True(serviceDto.TestGuidNullWithMixedValues == clientDto.TestGuidNullWithMixedValues);

            Assert.True(serviceDto.TestString == clientDto.TestString);

            Assert.True(serviceDto.TestStringNull == clientDto.TestStringNull);

            Assert.True(serviceDto.TestStringNullWithValue == clientDto.TestStringNullWithValue);

            Assert.True(serviceDto.TestStringNullWithMixedValues == clientDto.TestStringNullWithMixedValues);

            Assert.True(serviceDto.TestUShort == clientDto.TestUShort);

            Assert.True(serviceDto.TestUShortNull == clientDto.TestUShortNull);

            Assert.True(serviceDto.TestUShortNullWithValue == clientDto.TestUShortNullWithValue);

            Assert.True(serviceDto.TestUShortNullWithMixedValues == clientDto.TestUShortNullWithMixedValues);

            Assert.True(serviceDto.TestUInt == clientDto.TestUInt);

            Assert.True(serviceDto.TestUIntNull == clientDto.TestUIntNull);

            Assert.True(serviceDto.TestUIntNullWithValue == clientDto.TestUIntNullWithValue);

            Assert.True(serviceDto.TestUIntNullWithMixedValues == clientDto.TestUIntNullWithMixedValues);

            Assert.True(serviceDto.TestChar == clientDto.TestChar);

            Assert.True(serviceDto.TestCharNull == clientDto.TestCharNull);

            Assert.True(serviceDto.TestCharNullWithValue == clientDto.TestCharNullWithValue);

            Assert.True(serviceDto.TestCharNullWithMixedValues == clientDto.TestCharNullWithMixedValues);

            Assert.True(serviceDto.TestTimeSpan == clientDto.TestTimeSpan);

            Assert.True(serviceDto.TestTimeSpanNull == clientDto.TestTimeSpanNull);

            Assert.True(serviceDto.TestTimeSpanNullWithValue == clientDto.TestTimeSpanNullWithValue);

            Assert.True(serviceDto.TestTimeSpanNullWithMixedValues == clientDto.TestTimeSpanNullWithMixedValues);

            Assert.True(serviceDto.TestFloat == clientDto.TestFloat);

            Assert.True(serviceDto.TestFloatNull == clientDto.TestFloatNull);

            Assert.True(serviceDto.TestFloatNullWithValue == clientDto.TestFloatNullWithValue);

            Assert.True(serviceDto.TestFloatNullWithMixedValues == clientDto.TestFloatNullWithMixedValues);

            Assert.True(serviceDto.TestSbyte == clientDto.TestSbyte);

            Assert.True(serviceDto.TestSbyteNull == clientDto.TestSbyteNull);

            Assert.True(serviceDto.TestSbyteNullWithValue == clientDto.TestSbyteNullWithValue);

            Assert.True(serviceDto.TestSbyteNullWithMixedValues == clientDto.TestSbyteNullWithMixedValues);

        }
    }

    public class TestTestManager : TestManager<TestDto>
    {
        public override TestDto GetObjectNotFound()
        {
            return new TestDto
            {
                StorageKey = Guid.NewGuid().ToString().ToString()
            };
        }

        public override TestDto GetMinimumDataObject()
        {
            return new TestDto()
            {

            TestDateTime = DateTime.UtcNow,
            TestDateTimeOffset = DateTimeOffset.UtcNow,
            };
        }

        public override TestDto GetMaximumDataObject()
        {
            var model = new TestDto()
            {

            CreateDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            TestBool = true,
            TestBoolNull = true,
            TestBoolNullWithValue = true,
            TestBoolNullWithMixedValues = true,
            TestByte = 1,
            TestByteNull = 1,
            TestByteNullWithValue = 1,
            TestByteNullWithMixedValues = 1,
            TestByteArray = new byte[1] { 1 },
            TestByteArrayNull = new byte[1] { 1 },
            TestByteArrayNullWithValue = new byte[1] { 1 },
            TestByteArrayNullWithMixedValues = new byte[1] { 1 },
            TestShort = 1,
            TestShortNull = 1,
            TestShortNullWithValue = 1,
            TestShortNullWithMixedValues = 1,
            TestInt = 1,
            TestIntNull = 1,
            TestIntNullWithValue = 1,
            TestIntNullWithMixedValues = 1,
            TestLong = 1,
            TestLongNull = 1,
            TestLongNullWithValue = 1,
            TestLongNullWithMixedValues = 1,
            TestDateTime = DateTime.UtcNow,
            TestDateTimeNull = DateTime.UtcNow,
            TestDateTimeNullWithValue = DateTime.UtcNow,
            TestDateTimeNullWithMixedValues = DateTime.UtcNow,
            TestDateTimeOffset = DateTimeOffset.UtcNow,
            TestDateTimeOffsetNull = DateTimeOffset.UtcNow,
            TestDateTimeOffsetNullWithValue = DateTimeOffset.UtcNow,
            TestDateTimeOffsetNullWithMixedValues = DateTimeOffset.UtcNow,
            TestDateOnly = DateOnly.FromDateTime(DateTime.Today),
            TestDateOnlyNull = DateOnly.FromDateTime(DateTime.Today),
            TestDateOnlyNullWithValue = DateOnly.FromDateTime(DateTime.Today),
            TestDateOnlyNullWithMixedValues = DateOnly.FromDateTime(DateTime.Today),
            TestTimeOnly = TimeOnly.FromDateTime(DateTime.Now),
            TestTimeOnlyNull = TimeOnly.FromDateTime(DateTime.Now),
            TestTimeOnlyNullWithValue = TimeOnly.FromDateTime(DateTime.Now),
            TestTimeOnlyNullWithMixedValues = TimeOnly.FromDateTime(DateTime.Now),
            TestDouble = 1,
            TestDoubleNull = 1,
            TestDoubleNullWithValue = 1,
            TestDoubleNullWithMixedValues = 1,
            TestDecimal = 1,
            TestDecimalNull = 1,
            TestDecimalNullWithValue = 1,
            TestDecimalNullWithMixedValues = 1,
            TestGuid = Guid.NewGuid(),
            TestGuidNull = Guid.NewGuid(),
            TestGuidNullWithValue = Guid.NewGuid(),
            TestGuidNullWithMixedValues = Guid.NewGuid(),
            TestString = Guid.NewGuid().ToString(),
            TestStringNull = Guid.NewGuid().ToString(),
            TestStringNullWithValue = Guid.NewGuid().ToString(),
            TestStringNullWithMixedValues = Guid.NewGuid().ToString(),
            TestUShort = 1,
            TestUShortNull = 1,
            TestUShortNullWithValue = 1,
            TestUShortNullWithMixedValues = 1,
            TestUInt = 1,
            TestUIntNull = 1,
            TestUIntNullWithValue = 1,
            TestUIntNullWithMixedValues = 1,
            TestChar = 'b',
            TestCharNull = 'b',
            TestCharNullWithValue = 'b',
            TestCharNullWithMixedValues = 'b',
            TestTimeSpan = TimeSpan.FromDays(1),
            TestTimeSpanNull = TimeSpan.FromDays(1),
            TestTimeSpanNullWithValue = TimeSpan.FromDays(1),
            TestTimeSpanNullWithMixedValues = TimeSpan.FromDays(1),
            TestFloat = 1,
            TestFloatNull = 1,
            TestFloatNullWithValue = 1,
            TestFloatNullWithMixedValues = 1,
            TestSbyte = 1,
            TestSbyteNull = 1,
            TestSbyteNullWithValue = 1,
            TestSbyteNullWithMixedValues = 1,
            };
            return model;
        }

        public override IApiController<TestDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new TestApiController(serviceProvider.GetRequiredService<ITestApiService>(), options);
        }

        public override IApiController<TestDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new TestApiController(serviceProvider.GetRequiredService<ITestApiService>(), options);
        }

        public override IApiClient<TestDto> GetClient(IServiceProvider serviceProvider)
        {
            var appconfig = serviceProvider.GetRequiredService<IConfiguration>();
            var config = new ConfigurationBuilder()
                .AddConfiguration(appconfig)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "false" },
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ExposeSystemErrors", "true" }                    
                })
                .Build();

            return new TestApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                config);
        }

        public override IApiClient<TestDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            var appconfig = serviceProvider.GetRequiredService<IConfiguration>();
            var config = new ConfigurationBuilder()
                .AddConfiguration(appconfig)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "true" },
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ExposeSystemErrors", "true" }                    
                })
                .Build();

            return new TestApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                config);
        }

        public override IApiService<TestDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<ITestApiService>();
        }

        public override void UpdateObject(TestDto dto)
        {

            dto.TestBool = false;
            dto.TestBoolNull = false;
            dto.TestBoolNullWithValue = false;
            dto.TestBoolNullWithMixedValues = false;
            dto.TestByte = 0;
            dto.TestByteNull = 0;
            dto.TestByteNullWithValue = 0;
            dto.TestByteNullWithMixedValues = 0;
            dto.TestByteArray = new byte[1] { 0 };
            dto.TestByteArrayNull = new byte[1] { 0 };
            dto.TestByteArrayNullWithValue = new byte[1] { 0 };
            dto.TestByteArrayNullWithMixedValues = new byte[1] { 0 };
            dto.TestShort = 0;
            dto.TestShortNull = 0;
            dto.TestShortNullWithValue = 0;
            dto.TestShortNullWithMixedValues = 0;
            dto.TestInt = 0;
            dto.TestIntNull = 0;
            dto.TestIntNullWithValue = 0;
            dto.TestIntNullWithMixedValues = 0;
            dto.TestLong = 0;
            dto.TestLongNull = 0;
            dto.TestLongNullWithValue = 0;
            dto.TestLongNullWithMixedValues = 0;
            dto.TestDateTime = DateTime.UtcNow;
            dto.TestDateTimeNull = DateTime.UtcNow;
            dto.TestDateTimeNullWithValue = DateTime.UtcNow;
            dto.TestDateTimeNullWithMixedValues = DateTime.UtcNow;
            dto.TestDateTimeOffset = DateTimeOffset.UtcNow;
            dto.TestDateTimeOffsetNull = DateTimeOffset.UtcNow;
            dto.TestDateTimeOffsetNullWithValue = DateTimeOffset.UtcNow;
            dto.TestDateTimeOffsetNullWithMixedValues = DateTimeOffset.UtcNow;
            dto.TestDateOnly = DateOnly.FromDateTime(DateTime.Today);
            dto.TestDateOnlyNull = DateOnly.FromDateTime(DateTime.Today);
            dto.TestDateOnlyNullWithValue = DateOnly.FromDateTime(DateTime.Today);
            dto.TestDateOnlyNullWithMixedValues = DateOnly.FromDateTime(DateTime.Today);
            dto.TestTimeOnly = TimeOnly.FromDateTime(DateTime.Now);
            dto.TestTimeOnlyNull = TimeOnly.FromDateTime(DateTime.Now);
            dto.TestTimeOnlyNullWithValue = TimeOnly.FromDateTime(DateTime.Now);
            dto.TestTimeOnlyNullWithMixedValues = TimeOnly.FromDateTime(DateTime.Now);
            dto.TestDouble = 0;
            dto.TestDoubleNull = 0;
            dto.TestDoubleNullWithValue = 0;
            dto.TestDoubleNullWithMixedValues = 0;
            dto.TestDecimal = 0;
            dto.TestDecimalNull = 0;
            dto.TestDecimalNullWithValue = 0;
            dto.TestDecimalNullWithMixedValues = 0;
            dto.TestGuid = Guid.NewGuid();
            dto.TestGuidNull = Guid.NewGuid();
            dto.TestGuidNullWithValue = Guid.NewGuid();
            dto.TestGuidNullWithMixedValues = Guid.NewGuid();
            dto.TestString = Guid.NewGuid().ToString();
            dto.TestStringNull = Guid.NewGuid().ToString();
            dto.TestStringNullWithValue = Guid.NewGuid().ToString();
            dto.TestStringNullWithMixedValues = Guid.NewGuid().ToString();
            dto.TestUShort = 0;
            dto.TestUShortNull = 0;
            dto.TestUShortNullWithValue = 0;
            dto.TestUShortNullWithMixedValues = 0;
            dto.TestUInt = 0;
            dto.TestUIntNull = 0;
            dto.TestUIntNullWithValue = 0;
            dto.TestUIntNullWithMixedValues = 0;
            dto.TestChar = 'a';
            dto.TestCharNull = 'a';
            dto.TestCharNullWithValue = 'a';
            dto.TestCharNullWithMixedValues = 'a';
            dto.TestTimeSpan = TimeSpan.Zero;
            dto.TestTimeSpanNull = TimeSpan.Zero;
            dto.TestTimeSpanNullWithValue = TimeSpan.Zero;
            dto.TestTimeSpanNullWithMixedValues = TimeSpan.Zero;
            dto.TestFloat = 0;
            dto.TestFloatNull = 0;
            dto.TestFloatNullWithValue = 0;
            dto.TestFloatNullWithMixedValues = 0;
            dto.TestSbyte = 0;
            dto.TestSbyteNull = 0;
            dto.TestSbyteNullWithValue = 0;
            dto.TestSbyteNullWithMixedValues = 0;
        }

        public override void ValidateObjects(TestDto clientDto, TestDto serviceDto, HttpMethod method)
        {

            //PrimaryKeyRule
            if (method == HttpMethod.Post)
                Assert.True(!string.IsNullOrEmpty(serviceDto.StorageKey));
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);


            //CreateDateRule
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.CreateDate > clientDto.CreateDate);
            else
                Assert.True(serviceDto.CreateDate == clientDto.CreateDate);


            //UpdateDateRule
            if (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch)
                Assert.True(serviceDto.UpdateDate > clientDto.UpdateDate);
            else
                Assert.True(serviceDto.UpdateDate == clientDto.UpdateDate);


            Assert.True(serviceDto.TestBool == clientDto.TestBool);

            Assert.True(serviceDto.TestBoolNull == clientDto.TestBoolNull);

            Assert.True(serviceDto.TestBoolNullWithValue == clientDto.TestBoolNullWithValue);

            Assert.True(serviceDto.TestBoolNullWithMixedValues == clientDto.TestBoolNullWithMixedValues);

            Assert.True(serviceDto.TestByte == clientDto.TestByte);

            Assert.True(serviceDto.TestByteNull == clientDto.TestByteNull);

            Assert.True(serviceDto.TestByteNullWithValue == clientDto.TestByteNullWithValue);

            Assert.True(serviceDto.TestByteNullWithMixedValues == clientDto.TestByteNullWithMixedValues);

            Assert.True(serviceDto.TestByteArray != null && clientDto.TestByteArray != null ? serviceDto.TestByteArray.Length == clientDto.TestByteArray.Length : true);

            Assert.True(serviceDto.TestByteArrayNull != null && clientDto.TestByteArrayNull != null ? serviceDto.TestByteArrayNull.Length == clientDto.TestByteArrayNull.Length : true);

            Assert.True(serviceDto.TestByteArrayNullWithValue != null && clientDto.TestByteArrayNullWithValue != null ? serviceDto.TestByteArrayNullWithValue.Length == clientDto.TestByteArrayNullWithValue.Length : true);

            Assert.True(serviceDto.TestByteArrayNullWithMixedValues != null && clientDto.TestByteArrayNullWithMixedValues != null ? serviceDto.TestByteArrayNullWithMixedValues.Length == clientDto.TestByteArrayNullWithMixedValues.Length : true);

            Assert.True(serviceDto.TestShort == clientDto.TestShort);

            Assert.True(serviceDto.TestShortNull == clientDto.TestShortNull);

            Assert.True(serviceDto.TestShortNullWithValue == clientDto.TestShortNullWithValue);

            Assert.True(serviceDto.TestShortNullWithMixedValues == clientDto.TestShortNullWithMixedValues);

            Assert.True(serviceDto.TestInt == clientDto.TestInt);

            Assert.True(serviceDto.TestIntNull == clientDto.TestIntNull);

            Assert.True(serviceDto.TestIntNullWithValue == clientDto.TestIntNullWithValue);

            Assert.True(serviceDto.TestIntNullWithMixedValues == clientDto.TestIntNullWithMixedValues);

            Assert.True(serviceDto.TestLong == clientDto.TestLong);

            Assert.True(serviceDto.TestLongNull == clientDto.TestLongNull);

            Assert.True(serviceDto.TestLongNullWithValue == clientDto.TestLongNullWithValue);

            Assert.True(serviceDto.TestLongNullWithMixedValues == clientDto.TestLongNullWithMixedValues);

            Assert.True(serviceDto.TestDateTime == clientDto.TestDateTime);

            Assert.True(serviceDto.TestDateTimeNull == clientDto.TestDateTimeNull);

            Assert.True(serviceDto.TestDateTimeNullWithValue == clientDto.TestDateTimeNullWithValue);

            Assert.True(serviceDto.TestDateTimeNullWithMixedValues == clientDto.TestDateTimeNullWithMixedValues);

            Assert.True(serviceDto.TestDateTimeOffset == clientDto.TestDateTimeOffset);

            Assert.True(serviceDto.TestDateTimeOffsetNull == clientDto.TestDateTimeOffsetNull);

            Assert.True(serviceDto.TestDateTimeOffsetNullWithValue == clientDto.TestDateTimeOffsetNullWithValue);

            Assert.True(serviceDto.TestDateTimeOffsetNullWithMixedValues == clientDto.TestDateTimeOffsetNullWithMixedValues);

            Assert.True(serviceDto.TestDateOnly == clientDto.TestDateOnly);

            Assert.True(serviceDto.TestDateOnlyNull == clientDto.TestDateOnlyNull);

            Assert.True(serviceDto.TestDateOnlyNullWithValue == clientDto.TestDateOnlyNullWithValue);

            Assert.True(serviceDto.TestDateOnlyNullWithMixedValues == clientDto.TestDateOnlyNullWithMixedValues);

            Assert.True(serviceDto.TestTimeOnly == clientDto.TestTimeOnly);

            Assert.True(serviceDto.TestTimeOnlyNull == clientDto.TestTimeOnlyNull);

            Assert.True(serviceDto.TestTimeOnlyNullWithValue == clientDto.TestTimeOnlyNullWithValue);

            Assert.True(serviceDto.TestTimeOnlyNullWithMixedValues == clientDto.TestTimeOnlyNullWithMixedValues);

            Assert.True(serviceDto.TestDouble == clientDto.TestDouble);

            Assert.True(serviceDto.TestDoubleNull == clientDto.TestDoubleNull);

            Assert.True(serviceDto.TestDoubleNullWithValue == clientDto.TestDoubleNullWithValue);

            Assert.True(serviceDto.TestDoubleNullWithMixedValues == clientDto.TestDoubleNullWithMixedValues);

            Assert.True(serviceDto.TestDecimal == clientDto.TestDecimal);

            Assert.True(serviceDto.TestDecimalNull == clientDto.TestDecimalNull);

            Assert.True(serviceDto.TestDecimalNullWithValue == clientDto.TestDecimalNullWithValue);

            Assert.True(serviceDto.TestDecimalNullWithMixedValues == clientDto.TestDecimalNullWithMixedValues);

            Assert.True(serviceDto.TestGuid == clientDto.TestGuid);

            Assert.True(serviceDto.TestGuidNull == clientDto.TestGuidNull);

            Assert.True(serviceDto.TestGuidNullWithValue == clientDto.TestGuidNullWithValue);

            Assert.True(serviceDto.TestGuidNullWithMixedValues == clientDto.TestGuidNullWithMixedValues);

            Assert.True(serviceDto.TestString == clientDto.TestString);

            Assert.True(serviceDto.TestStringNull == clientDto.TestStringNull);

            Assert.True(serviceDto.TestStringNullWithValue == clientDto.TestStringNullWithValue);

            Assert.True(serviceDto.TestStringNullWithMixedValues == clientDto.TestStringNullWithMixedValues);

            Assert.True(serviceDto.TestUShort == clientDto.TestUShort);

            Assert.True(serviceDto.TestUShortNull == clientDto.TestUShortNull);

            Assert.True(serviceDto.TestUShortNullWithValue == clientDto.TestUShortNullWithValue);

            Assert.True(serviceDto.TestUShortNullWithMixedValues == clientDto.TestUShortNullWithMixedValues);

            Assert.True(serviceDto.TestUInt == clientDto.TestUInt);

            Assert.True(serviceDto.TestUIntNull == clientDto.TestUIntNull);

            Assert.True(serviceDto.TestUIntNullWithValue == clientDto.TestUIntNullWithValue);

            Assert.True(serviceDto.TestUIntNullWithMixedValues == clientDto.TestUIntNullWithMixedValues);

            Assert.True(serviceDto.TestChar == clientDto.TestChar);

            Assert.True(serviceDto.TestCharNull == clientDto.TestCharNull);

            Assert.True(serviceDto.TestCharNullWithValue == clientDto.TestCharNullWithValue);

            Assert.True(serviceDto.TestCharNullWithMixedValues == clientDto.TestCharNullWithMixedValues);

            Assert.True(serviceDto.TestTimeSpan == clientDto.TestTimeSpan);

            Assert.True(serviceDto.TestTimeSpanNull == clientDto.TestTimeSpanNull);

            Assert.True(serviceDto.TestTimeSpanNullWithValue == clientDto.TestTimeSpanNullWithValue);

            Assert.True(serviceDto.TestTimeSpanNullWithMixedValues == clientDto.TestTimeSpanNullWithMixedValues);

            Assert.True(serviceDto.TestFloat == clientDto.TestFloat);

            Assert.True(serviceDto.TestFloatNull == clientDto.TestFloatNull);

            Assert.True(serviceDto.TestFloatNullWithValue == clientDto.TestFloatNullWithValue);

            Assert.True(serviceDto.TestFloatNullWithMixedValues == clientDto.TestFloatNullWithMixedValues);

            Assert.True(serviceDto.TestSbyte == clientDto.TestSbyte);

            Assert.True(serviceDto.TestSbyteNull == clientDto.TestSbyteNull);

            Assert.True(serviceDto.TestSbyteNullWithValue == clientDto.TestSbyteNullWithValue);

            Assert.True(serviceDto.TestSbyteNullWithMixedValues == clientDto.TestSbyteNullWithMixedValues);

        }

        public override List<ServiceQueryRequest> GetQueriesForObject(TestDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();
            IServiceQueryRequestBuilder qb = null;


            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.StorageKey), dto.StorageKey.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.CreateDate), dto.CreateDate.ToString("o"));
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.UpdateDate), dto.UpdateDate.ToString("o"));
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestBool), dto.TestBool.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestBoolNull), dto.TestBoolNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestBoolNullWithValue), dto.TestBoolNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestBoolNullWithMixedValues), dto.TestBoolNullWithMixedValues.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestByte), dto.TestByte.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestByteNull), dto.TestByteNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestByteNullWithValue), dto.TestByteNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestByteNullWithMixedValues), dto.TestByteNullWithMixedValues.ToString());
            queries.Add(qb.Build());
// Property TestByteArray unsupported for querying
// Property TestByteArrayNull unsupported for querying
// Property TestByteArrayNullWithValue unsupported for querying
// Property TestByteArrayNullWithMixedValues unsupported for querying

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestShort), dto.TestShort.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestShortNull), dto.TestShortNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestShortNullWithValue), dto.TestShortNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestShortNullWithMixedValues), dto.TestShortNullWithMixedValues.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestInt), dto.TestInt.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestIntNull), dto.TestIntNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestIntNullWithValue), dto.TestIntNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestIntNullWithMixedValues), dto.TestIntNullWithMixedValues.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestLong), dto.TestLong.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestLongNull), dto.TestLongNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestLongNullWithValue), dto.TestLongNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestLongNullWithMixedValues), dto.TestLongNullWithMixedValues.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDateTime), dto.TestDateTime.ToString("o"));
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDateTimeNull), dto.TestDateTimeNull.Value.ToString("o"));
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDateTimeNullWithValue), dto.TestDateTimeNullWithValue.Value.ToString("o"));
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDateTimeNullWithMixedValues), dto.TestDateTimeNullWithMixedValues.Value.ToString("o"));
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDateTimeOffset), dto.TestDateTimeOffset.ToString("o"));
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDateTimeOffsetNull), dto.TestDateTimeOffsetNull.Value.ToString("o"));
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDateTimeOffsetNullWithValue), dto.TestDateTimeOffsetNullWithValue.Value.ToString("o"));
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDateTimeOffsetNullWithMixedValues), dto.TestDateTimeOffsetNullWithMixedValues.Value.ToString("o"));
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDateOnly), dto.TestDateOnly.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDateOnlyNull), dto.TestDateOnlyNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDateOnlyNullWithValue), dto.TestDateOnlyNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDateOnlyNullWithMixedValues), dto.TestDateOnlyNullWithMixedValues.ToString());
            queries.Add(qb.Build());
// Property TestTimeOnly unsupported for querying
// Property TestTimeOnlyNull unsupported for querying
// Property TestTimeOnlyNullWithValue unsupported for querying
// Property TestTimeOnlyNullWithMixedValues unsupported for querying

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDouble), dto.TestDouble.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDoubleNull), dto.TestDoubleNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDoubleNullWithValue), dto.TestDoubleNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDoubleNullWithMixedValues), dto.TestDoubleNullWithMixedValues.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDecimal), dto.TestDecimal.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDecimalNull), dto.TestDecimalNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDecimalNullWithValue), dto.TestDecimalNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestDecimalNullWithMixedValues), dto.TestDecimalNullWithMixedValues.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestGuid), dto.TestGuid.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestGuidNull), dto.TestGuidNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestGuidNullWithValue), dto.TestGuidNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestGuidNullWithMixedValues), dto.TestGuidNullWithMixedValues.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestString), dto.TestString.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestStringNull), dto.TestStringNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestStringNullWithValue), dto.TestStringNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestStringNullWithMixedValues), dto.TestStringNullWithMixedValues.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestUShort), dto.TestUShort.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestUShortNull), dto.TestUShortNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestUShortNullWithValue), dto.TestUShortNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestUShortNullWithMixedValues), dto.TestUShortNullWithMixedValues.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestUInt), dto.TestUInt.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestUIntNull), dto.TestUIntNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestUIntNullWithValue), dto.TestUIntNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestUIntNullWithMixedValues), dto.TestUIntNullWithMixedValues.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestChar), dto.TestChar.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestCharNull), dto.TestCharNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestCharNullWithValue), dto.TestCharNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestCharNullWithMixedValues), dto.TestCharNullWithMixedValues.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestTimeSpan), dto.TestTimeSpan.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestTimeSpanNull), dto.TestTimeSpanNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestTimeSpanNullWithValue), dto.TestTimeSpanNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestTimeSpanNullWithMixedValues), dto.TestTimeSpanNullWithMixedValues.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestFloat), dto.TestFloat.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestFloatNull), dto.TestFloatNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestFloatNullWithValue), dto.TestFloatNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestFloatNullWithMixedValues), dto.TestFloatNullWithMixedValues.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestSbyte), dto.TestSbyte.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestSbyteNull), dto.TestSbyteNull.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestSbyteNullWithValue), dto.TestSbyteNullWithValue.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(TestDto.TestSbyteNullWithMixedValues), dto.TestSbyteNullWithMixedValues.ToString());
            queries.Add(qb.Build());


            return queries;
        }
    }
}
