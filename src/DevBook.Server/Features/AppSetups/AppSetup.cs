﻿using DevBook.Server.Common;

namespace DevBook.Server.Features.AppSetups;

internal sealed record AppSetup(
	string Name,
	string Path,
	string? Arguments)
	: Entity(Guid.NewGuid());