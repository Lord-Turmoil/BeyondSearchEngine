// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Tonisoft.AspExtensions.Response;

public class ApiRequestDto
{
    public virtual bool Verify()
    {
        return true;
    }

    public virtual ApiRequestDto Format()
    {
        return this;
    }
}