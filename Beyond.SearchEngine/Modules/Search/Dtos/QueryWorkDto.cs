﻿// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Newtonsoft.Json;

namespace Beyond.SearchEngine.Modules.Search.Dtos;

public class TimeRangeData
{
    /// <summary>
    ///     Inclusive.
    /// </summary>
    public DateTime From { get; set; }

    /// <summary>
    ///     Inclusive.
    /// </summary>
    public DateTime To { get; set; }
}

public class OrderByData
{
    public string Field { get; set; }
    public bool Ascending { get; set; }
}

public class BasicCondition
{
    public string Field { get; set; }
    public string Value { get; set; }
}

public class AdvancedCondition
{
    public string Field { get; set; }
    public string Value { get; set; }

    /// <summary>
    ///     Can be "and", "or", "not".
    /// </summary>
    public string Op { get; set; }

    public bool Fuzzy { get; set; }
}

public class QueryWorkDto : PagedRequestDto
{
    /// <summary>
    ///     Whether to return brief information.
    /// </summary>
    public bool Brief { get; set; }

    public OrderByData? OrderBy { get; set; }
    public TimeRangeData? TimeRange { get; set; }
    public List<string> Concepts { get; set; }

    public override bool Verify()
    {
        if (!base.Verify())
        {
            return false;
        }

        if (TimeRange != null && TimeRange.From > TimeRange.To)
        {
            return false;
        }

        return OrderBy == null || Globals.AvailableSortFiled.Contains(OrderBy.Field);
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}

public class QueryWorkBasicDto : QueryWorkDto
{
    public List<BasicCondition> Conditions { get; set; }

    public override bool Verify()
    {
        return base.Verify() && Conditions.Count > 0;
    }
}

public class QueryWorkAdvancedDto : QueryWorkDto
{
    public List<AdvancedCondition> Conditions { get; set; }

    public override bool Verify()
    {
        return base.Verify() && Conditions.Count > 0;
    }
}

public class QueryWorkAllFieldsDto : PagedRequestDto
{
    public bool Brief { get; set; }

    public OrderByData? OrderBy { get; set; }
    public TimeRangeData? TimeRange { get; set; }
    public string Query { get; set; }

    public override bool Verify()
    {
        if (!base.Verify())
        {
            return false;
        }

        if (TimeRange != null && TimeRange.From > TimeRange.To)
        {
            return false;
        }

        if (OrderBy != null && !Globals.AvailableSortFiled.Contains(OrderBy.Field))
        {
            return false;
        }

        return !string.IsNullOrWhiteSpace(Query);
    }
}