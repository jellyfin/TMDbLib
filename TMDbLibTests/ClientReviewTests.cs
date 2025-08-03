﻿using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Reviews;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using System.Globalization;

namespace TMDbLibTests;

public class ClientReviewTests : TestBase
{
    [Fact]
    public async Task TestReviewFullDetails()
    {
        Review review = await TMDbClient.GetReviewAsync(IdHelper.TheDarkKnightRisesReviewId);

        await Verify(review);
    }
    [Fact]
    public async Task TestReviewMissing()
    {
        Review review = await TMDbClient.GetReviewAsync(IdHelper.MissingID.ToString(CultureInfo.InvariantCulture));

        Assert.Null(review);
    }
}
