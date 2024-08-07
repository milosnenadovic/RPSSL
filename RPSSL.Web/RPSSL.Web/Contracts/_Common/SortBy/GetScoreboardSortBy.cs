﻿using System.ComponentModel;

namespace RPSSL.Web.Contracts._Common.SortBy;

public enum GetScoreboardSortBy
{
    [Description("PlayerChoice")]
    PlayerChoice = 1,
    [Description("ComputerChoice")]
    ComputerChoice = 2,
    [Description("PlayedAt")]
    PlayedAt = 3,
    [Description("Result")]
    Result = 4
}