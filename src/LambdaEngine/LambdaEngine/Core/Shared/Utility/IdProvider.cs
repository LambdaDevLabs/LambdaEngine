namespace LambdaEngine;

public class IdProvider {
    private int nextId;
    private readonly int poolSize;
    private readonly int idRecycleBufferSize;
    private readonly Stack<int> idPool;

    public IdProvider(int poolSize = 100, int idRecycleBufferSize = 10) {
        this.poolSize = poolSize;
        this.idRecycleBufferSize = idRecycleBufferSize;
        idPool = new Stack<int>(poolSize + idRecycleBufferSize);
        
        FillIdPool();
    }

    /// <summary>
    /// Retrieves the next id from the pool.
    /// </summary>
    /// <returns></returns>
    public int NextId() {
        if (idPool.Count > 0) {
            return idPool.Pop();
        }
        
        FillIdPool();
        return idPool.Pop();
    }

    /// <summary>
    /// <para>Frees an id to be reused.</para>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="validateId"></param>
    public void FreeId(int id, bool validateId = false) {
        if (validateId) {
            if (!idPool.Contains(id)) {
                idPool.Push(id);
            }
        }
        else {
            idPool.Push(id);
        }
    }

    /// <summary>
    /// <para>Adds the next poolSize ids to the pool.</para>
    /// <para>Generally, this method should not be called manually.</para>
    /// </summary>
    public void FillIdPool() {
        idPool.EnsureCapacity(idPool.Count + poolSize + idRecycleBufferSize);
        
        for (int i = poolSize - 1; i >= 0; i--) {
            idPool.Push(nextId + i);
        }

        nextId += poolSize;
    }
}