using System.Collections;

namespace Wisedev.Magic.Titam.Utils;

public class LogicArrayList<T> : IEnumerable<T>
{
    private T[] m_items;
    private int m_count;

    public LogicArrayList()
    {
        m_items = new T[0];
    }

    public LogicArrayList(int initialCapacity)
    {
        m_items = new T[initialCapacity];
    }

    public int Count => m_count;

    public T this[int index]
    {
        get
        {
            return m_items[index];
        }
        set
        {
            m_items[index] = value;
        }
    }

    public void Add(T item)
    {
        int length = m_items.Length;

        if (length == m_count)
            EnsureCapacity(length != 0 ? length * 2 : 5);

        m_items[m_count++] = item;
    }

    public void Add(int index, T item)
    {
        int length = m_items.Length;

        if (length == m_count)
            EnsureCapacity(length != 0 ? length * 2 : 5);

        if (m_count > index)
            Array.Copy(m_items, index, m_items, index + 1, m_count - index);

        m_items[index] = item;
        m_count += 1;
    }

    public int IndexOf(T item)
    {
        return Array.IndexOf(m_items, item, 0, m_count);
    }

    public void Remove(int index)
    {
        if (index < m_count)
        {
            m_count -= 1;

            if (index != m_count)
                Array.Copy(m_items, index, m_items, index + 1, m_count - index);
        }
    }

    public void EnsureCapacity(int count)
    {
        int size = m_items.Length;

        if (size < count)
            Array.Resize(ref m_items, count);
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < m_count; i++)
        {
            yield return m_items[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
