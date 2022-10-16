using System;

namespace Zs.Bot.Data.Abstractions;

/// <summary>Database entity</summary>
/// <typeparam name="TKey">Primary key type</typeparam>
public interface IDbEntity<TEntity, TKey>
    where TKey: notnull
{
    TKey Id { get; set; }

    // TODO: Create specific delegate types

    /// <summary> Returns the entity prepared for saving to DB </summary>
    Func<TEntity> GetItemForSave { get; }

    /// <summary> Helps to update only fields you want and do not change others.
    /// Argument - item existing in database </summary>
    Func<TEntity, TEntity> GetItemForUpdate { get; }
}
