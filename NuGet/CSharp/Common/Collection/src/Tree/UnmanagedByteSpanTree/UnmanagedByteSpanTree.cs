using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HS.CSharp.Common.Collection;

/// <summary>
/// 내부는 참조 변수로 이루어져 있기 때문에 복사하여도 정보를 공유함
/// </summary>
public partial struct UnmanagedByteSpanTree<TValue>
    where TValue : unmanaged
{
    #region Object
    
    public struct FindData
    {
        public int creatNodeCount;

        public FindData()
        {
            this.creatNodeCount = 0;
        }
    }
    
    unsafe internal struct ByteNode
    {
        #region Instance

        public readonly byte keyByte;
        
        TValue value;
        public TValue Value {
            get => value;
            set => SetValue(value);
        }

        bool hasValue;
        public bool HasValue => hasValue;


        #region 생성자

        /// <summary>
        /// 값 없는 노드 생성
        /// </summary>
        /// <param name="keyByte"></param>
        public ByteNode(byte keyByte)
        {
            this.keyByte = keyByte;

            this.value = default;
            this.hasValue = false;
        }

        /// <summary>
        /// 값 있는 노드 생성
        /// </summary>
        /// <param name="keyByte"></param>
        public ByteNode(byte keyByte, TValue value)
        {
            this.keyByte = keyByte;

            this.value = value;
            this.hasValue = true;
        }
    
        #endregion


        #region Method

        public bool TryGetValue(out TValue value)
        {
            if (hasValue)
            {
                value = this.value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
        public void SetValue(TValue value)
        {
            this.value = value;
            hasValue = true;
        }

        #endregion

        #endregion
    }

    unsafe public ref struct Enumerator
    {
        UnmanagedTree<ByteNode>.Enumerator nodeEnumerator;
        ByteNode* valueNodePtr;

        public Enumerator(UnmanagedByteSpanTree<TValue> tree)
        {
            this.nodeEnumerator = tree.GetNodeEnumerator();
        }
        
        public bool MoveNext()
        {
            while (nodeEnumerator.MoveNext())
            {
                ByteNode* currentNodePtr = &nodeEnumerator.CurrentPtr->value;

                // 찾은 노드가 값이 있음 -> 현재 노드 설정 후 true 반환
                if (currentNodePtr->HasValue)
                {
                    valueNodePtr = currentNodePtr;
                    return true;
                }
            }
            return false;
        }

        public void Dispose()
        {
            nodeEnumerator.Dispose();
        }
    }
    
    #endregion


    #region Instance

    #region 필드

    UnmanagedTree<ByteNode> byteNodeTree;

    public int nodeCount;
    public int valueCount;

    #endregion


    // 생성자
    public UnmanagedByteSpanTree()
    {
        byteNodeTree = new();
    }


    #region 메서드

    unsafe public void Add(ReadOnlySpan<byte> key, TValue value)
    {
        int keyLength = key.Length;

        if (keyLength == 0)
            throw new ArgumentException("키의 길이는 0보다 길어야 합니다.");

        FindData findData = new FindData();

        byte keyByte;
        UnmanagedTree<ByteNode>.UnmanagedTreeNode* curTreeNodePtr = byteNodeTree.rootNode;
        
        for (int i = 0; i < keyLength; i++)
        {
            keyByte = key[i];

            curTreeNodePtr = GetOrCreateChild(curTreeNodePtr, keyByte, ref findData);
        }

        nodeCount += findData.creatNodeCount;

        ByteNode* findByteNodePtr = &curTreeNodePtr->value;

        if (findByteNodePtr->HasValue)
            throw new ArgumentException($"'{Encoding.ASCII.GetString(key)}' 키는 이미 존재합니다.");
        
        findByteNodePtr->SetValue(value);
        valueCount++;
    }
    unsafe public void AddOrSet(ReadOnlySpan<byte> key, TValue value)
    {
        int keyLength = key.Length;

        if (keyLength == 0)
            throw new ArgumentException("키의 길이는 0보다 길어야 합니다.");

        FindData findData = new FindData();

        byte keyByte;
        UnmanagedTree<ByteNode>.UnmanagedTreeNode* curTreeNodePtr = byteNodeTree.rootNode;
        
        for (int i = 0; i < keyLength; i++)
        {
            keyByte = key[i];

            curTreeNodePtr = GetOrCreateChild(curTreeNodePtr, keyByte, ref findData);
        }

        nodeCount += findData.creatNodeCount;

        ByteNode* findByteNodePtr = &curTreeNodePtr->value;

        if (findByteNodePtr->HasValue == false)
        {
            // 값이 없음 -> 값 개수 증가
            valueCount++;
        }
        
        findByteNodePtr->SetValue(value);
    }
    unsafe static UnmanagedTree<ByteNode>.UnmanagedTreeNode* GetOrCreateChild(UnmanagedTree<ByteNode>.UnmanagedTreeNode* nodePtr, byte keyByte, ref FindData findData)
    {
        UnmanagedTree<ByteNode>.UnmanagedTreeNode* closestChildNodePtr = GetClosestKeyByteChildNodePtr(nodePtr, keyByte);

        if (closestChildNodePtr == null)
        {
            ByteNode newByteNode = new (keyByte);
            findData.creatNodeCount++;

            return nodePtr->AddChildFirstAndReturnPtr(newByteNode);
        }
        else
        {
            // 이미 자식 노드가 존재함 => 포인터 반환
            if (closestChildNodePtr->value.keyByte == keyByte)
            {
                return closestChildNodePtr;
            }
            // 자식 노드가 없음
            else
            {
                ByteNode newByteNode = new (keyByte);
                findData.creatNodeCount++;

                return nodePtr->InsertChildNextAndReturnPtr(closestChildNodePtr, newByteNode);
            }
        }


        UnmanagedLinkedList<UnmanagedTree<ByteNode>.UnmanagedTreeNode>* childListPtr = &(nodePtr->childNodeList);
        int childCount = childListPtr->Count;
        
        // 자식이 없음 => 자식 노드 생성 후 반환
        if (childCount == 0)
        {
            ByteNode newByteNode = new (keyByte);
            findData.creatNodeCount++;
            
            return nodePtr->AddChildFirstAndReturnPtr(newByteNode);
        }

        // 자식이 1개임
        if (childCount == 1)
        {
            // 자식 노드 포인터
            UnmanagedTree<ByteNode>.UnmanagedTreeNode* childNodePtr = childListPtr->HeadValuePtr;
            
            // 키 바이트가 같음 => 포인터 반환
            if (childNodePtr->value.keyByte == keyByte)
            {
                return childNodePtr;
            }
            // 키 바이트가 다름 => 자식 노드 생성 후 반환
            else
            {
                ByteNode newByteNode = new (keyByte);
                findData.creatNodeCount++;

                // 바이트가 작음 => 앞에 삽입
                if (keyByte < childNodePtr->value.keyByte)
                {
                    return nodePtr->AddChildFirstAndReturnPtr(newByteNode);
                }
                // 바이트가 큼 => 뒤에 삽입
                else
                {
                    return nodePtr->AddChildLastAndReturnPtr(newByteNode);
                }
            }
        }


        // 열거자 생성
        UnmanagedLinkedList<UnmanagedTree<ByteNode>.UnmanagedTreeNode>.PointerEnumerator childListRefEnumerator 
            = new (childListPtr);

        // 첫번째 노드
        if (childListRefEnumerator.MoveNext() == false)
        {
            // 자식이 없음 => 자식 노드 생성 후 반환
            ByteNode newByteNode = new ByteNode(keyByte);
            findData.creatNodeCount++;
            
            return nodePtr->AddChildFirstAndReturnPtr(newByteNode);
        }

        scoped UnmanagedLinkedList<UnmanagedTree<ByteNode>.UnmanagedTreeNode>.PointerEnumerator.EnumerationInfo curInfo = childListRefEnumerator.Info;
        UnmanagedTree<ByteNode>.UnmanagedTreeNode curTreeNode = curInfo.Value;

        scoped UnmanagedLinkedList<UnmanagedTree<ByteNode>.UnmanagedTreeNode>.PointerEnumerator.EnumerationInfo prevInfo;
            
        while (childListRefEnumerator.MoveNext())
        {
            prevInfo = curInfo;

            curInfo = childListRefEnumerator.Info;
            curTreeNode = curInfo.Value;

            // 아직 찾지 못함 : keyByte < cur
            if (curTreeNode.value.keyByte > keyByte)
            {
                continue;
            }

            // 찾음 : keyByte == cur
            if (curTreeNode.value.keyByte == keyByte)
            {
                // 이미 자식 노드가 존재함 -> 참조 반환
                return curInfo.ValuePtr;
            }

            // 현재 노드가 keyByte보다 큼
            findData.creatNodeCount++;

            // 이전 노드 다음에 삽입 및 참조 반환
            return nodePtr->InsertChildNextAndReturnPtr(prevInfo, new ByteNode(keyByte));
        }

        // keyByte보다 크거나 같은 자식이 존재하지 않음 => 마지막에 삽입
        findData.creatNodeCount++;
        
        return nodePtr->AddChildLastAndReturnPtr(new ByteNode(keyByte));
    }
    /// <summary>
    /// 작거나 같은 자식 노드 중 가장 큰 노드의 포인터를 반환함
    /// </summary>
    /// <param name="nodePtr"></param>
    /// <param name="keyByte"></param>
    /// <returns></returns>
    unsafe static UnmanagedLinkedList<UnmanagedTree<ByteNode>.UnmanagedTreeNode>.UnmanagedLinkedListNode* GetClosestKeyByteChildNodePtr(UnmanagedTree<ByteNode>.UnmanagedTreeNode* nodePtr, byte keyByte)
    {
        UnmanagedLinkedList<UnmanagedTree<ByteNode>.UnmanagedTreeNode>* childListPtr = &(nodePtr->childNodeList);
        int childCount = childListPtr->Count;
        
        // 자식이 없음 => null 반환
        if (childCount == 0)
        {
            return null;
        }

        // 자식이 1개임
        if (childCount == 1)
        {
            UnmanagedLinkedList<UnmanagedTree<ByteNode>.UnmanagedTreeNode>.UnmanagedLinkedListNode* childListNodePtr = childListPtr->HeadNodePtr;

            // 키 바이트보다 큼 => null 반환
            if (childListNodePtr->value.value.keyByte > keyByte)
            {
                return null;
            }
            // 키 바이트보다 작거나 같음 => 포인터 반환
            else
            {
                return childListNodePtr;
            }
        }


        // 열거자 생성
        UnmanagedLinkedList<UnmanagedTree<ByteNode>.UnmanagedTreeNode>.PointerEnumerator childListRefEnumerator 
            = new (childListPtr);

        // 첫번째 노드
        if (childListRefEnumerator.MoveNext() == false)
        {
            // 자식이 없음 => null 반환
            return null;
        }

        UnmanagedLinkedList<UnmanagedTree<ByteNode>.UnmanagedTreeNode>.UnmanagedLinkedListNode* childListNodePtr = childListPtr->HeadNodePtr;
        UnmanagedTree<ByteNode>.UnmanagedTreeNode* curTreeNodePtr = curInfo.ValuePtr;
        byte curTreeNodeKeyByte;

        UnmanagedTree<ByteNode>.UnmanagedTreeNode* prevTreeNodePtr;
            
        while (childListRefEnumerator.MoveNext())
        {
            // 이전 정보 임시 저장
            prevTreeNodePtr = curTreeNodePtr;

            curInfo = childListRefEnumerator.Info;
            curTreeNodePtr = curInfo.ValuePtr;
            curTreeNodeKeyByte = curTreeNodePtr->value.keyByte;

            // 아직 찾지 못함 : keyByte < cur
            if (curTreeNodeKeyByte > keyByte)
            {
                continue;
            }

            // 찾음 : keyByte == cur
            if (curTreeNodeKeyByte == keyByte)
            {
                // 이미 자식 노드가 존재함 -> 현재 포인터 반환
                return curTreeNodePtr;
            }

            // 현재 노드가 keyByte보다 큼 => 이전 포인터 반환
            return prevTreeNodePtr;
        }
        
        // keyByte보다 크거나 같은 자식이 존재하지 않음 => 마지막 포인터 반환
        return curTreeNodePtr;
    }
    public TValue GetValue(ReadOnlySpan<byte> key)
    {
        ByteNode node = GetNode(key);

        if (node.valueInfo == null)
        {
            throw new KeyNotFoundException($"값이 존재하지 않습니다.");
        }
        else
        {
            return node.valueInfo.value;
        }
    }
    public bool TryGetValue(ReadOnlySpan<byte> key, [NotNullWhen(true)] out TValue? value)
    {
        if (TryGetNode(key, out ByteNode? node))
        {
            if (node.valueInfo == null)
            {
                value = default;
                return false;
            }
            else
            {
                value = node.valueInfo.value!;
                return true;
            }
        }
        else
        {
            value = default;
            return false;
        }
    }
    
    public Enumerator GetEnumerator()
        => new Enumerator(this);
    public TValue[] ToArray()
    {
        TValue[] valueArray = new TValue[valueCount];

        using (Enumerator enumerator = GetEnumerator())
        {
            for (int i = 0; i < valueCount; i++)
            {
                if (enumerator.MoveNext())
                {
                    valueArray[i] = enumerator.;
                }
                else
                {
                    throw new Exception("트리가 잘못되었습니다.");
                }
            }
        }

        return valueArray;
    }

    public void Dispose()
    {
        // 노드 리스트를 Dispose
        byteNodeTree.Dispose();
    }



    internal UnmanagedTree<ByteNode>.Enumerator GetNodeEnumerator()
        => byteNodeTree.GetEnumerator();

    internal ByteNode GetNode(ReadOnlySpan<byte> key)
    {
        if (TryGetNode(key, out ByteNode? node))
        {
            return node;
        }
        else
        {
            throw new KeyNotFoundException($"'{Encoding.ASCII.GetString(key)}' 키를 찾을 수 없습니다.");
        }
    }

    internal bool TryGetNode(ReadOnlySpan<byte> key, [NotNullWhen(true)] out ByteNode? node)
    {
        // 키의 길이가 0임 -> false 반환
        if (key.Length == 0) {
            node = null;
            return false;
        }

        ByteNode? findNode = rootNode;
        int keySliceStartIndex = 0;
           
        while (keySliceStartIndex < key.Length)
        {
            int findNodeKeySliceLength = findNode.keySlice.Length;

            // 자식 찾기
            if (findNode.TryFindChildNode(key.Slice(keySliceStartIndex, findNodeKeySliceLength), out findNode))
            {
                // 키 조각 시작 인덱스 업데이트
                keySliceStartIndex += findNodeKeySliceLength;
                continue;
            }
            else
            {
                // 자식을 찾을 수 없음
                node = null;
                return false;
            }
        }

        node = null;
        return false;
    }

    #endregion

    #endregion
}