using System;
using System.Collections.Generic;

namespace QCS
{
    /// <summary>
    /// Classe représentant un circuit ce qui comprend:
    ///     - une liste de qubits qui constituent l'entrée du circuit
    ///     - un tableau 2D contenant les portes du circuit, les portes de plusieurs entrées occupent les cases juxtaposées nécessaires
    ///     
    /// La dénomination "vide" correspond à l'omniprésence de la porte IDENTITY.
    /// </summary>
    public class Circuit
    {
        /// <summary>
        /// Classe regroupant les données d'une entrée au sein d'un cricuit, à savoir sa colone "col" et sa valeur "qubit".
        /// </summary>
        public class EntryStruct
        {
            public int col
            {
                internal set;
                get;
            }
            public Qubit qubit
            {
                internal set;
                get;
            }

            public EntryStruct(int col, Qubit qubit)
            {
                this.col = col;
                this.qubit = qubit;
            }
        }

        /// <summary>
        /// Classe regroupant les données du'une porte au sein d'un cricuit, à savoir sa ligne "row", sa colone "col" et la porte en question "gate".
        /// </summary>
        public class GateStruct
        {
            public int row
            {
                internal set;
                get;
            }
            public int col
            {
                internal set;
                get;
            }
            public Gate gate
            {
                internal set;
                get;
            }

            public GateStruct(int row, int col, Gate gate)
            {
                this.row = row;
                this.col = col;
                this.gate = gate;
            }
        }

        public List<EntryStruct> entries;
        public List<List<GateStruct>> rows;

        public delegate void EntryEventHandler(EntryStruct entry);
        public delegate void GateEventHandler(GateStruct gateStruct);
        public delegate void RowEventHandler(List<GateStruct> row);
        public delegate void ColEventHandler(EntryStruct entry, List<GateStruct> col);
        
        public EntryEventHandler OnSetEntry;
        public GateEventHandler OnCreateGate;
        public GateEventHandler OnPutGate;
        public GateEventHandler OnMoveGate;
        public GateEventHandler OnRemoveGate;
        public RowEventHandler OnInsertRow;
        public RowEventHandler OnMoveRow;
        public RowEventHandler OnRemoveRow;
        public ColEventHandler OnInsertCol;
        public ColEventHandler OnMoveCol;
        public ColEventHandler OnRemoveCol;

        /// <summary>
        /// Renvoie le nombre de colones (alias le nombre d'entrées du circuit)
        /// </summary>
        public int NbCol
        {
            get { return entries.Count; }
        }
        /// <summary>
        /// Renvoie le nombre de lignes du circuit
        /// </summary>
        public int NbRow
        {
            get { return rows.Count; }
        }

        /// <summary>
        /// Crée un circuit vide.
        /// </summary>
        public Circuit()
        {
            entries = new List<EntryStruct>();
            rows = new List<List<GateStruct>>();
        }
        /// <summary>
        /// Crée un circuit vide aux dimensions spécifiées.
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        public Circuit(int rowCount, int colCount) : this()
        {
            this.OnSetEntry = null;
            this.OnCreateGate = null;
            this.OnPutGate = null;
            this.OnMoveGate = null;
            this.OnRemoveGate = null;
            this.OnInsertRow = null;
            this.OnMoveRow = null;
            this.OnRemoveRow = null;
            this.OnInsertCol = null;
            this.OnMoveCol = null;
            this.OnRemoveCol = null;

            EnsureCapacity(rowCount, colCount);
        }

        public void EnsureCapacity(int rowCount, int colCount)
        {
            for (int i = NbCol; i < colCount; i++)
                InsertCol(i);

            for (int i = NbRow; i < rowCount; i++)
                InsertRow(i);
        }

        /**
	     * ####################################################################################################
	     * ############################################## ENTRY ###############################################
	     * ####################################################################################################
	     */

        /// <summary>
        /// Crée un EntryStruct et appelle l'évennement OnCreateEntry.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="qubit"></param>
        /// <returns>EntryStruct</returns>
        private EntryStruct CreateEntryStruct(int col, Qubit qubit)
        {
            EntryStruct entryStruct = new EntryStruct(col, qubit);
            return entryStruct;
        }
        
        /// <summary>
        /// Retourne l'EntryStruct de la colone spécifiée
        /// </summary>
        /// <param name="col"></param>
        /// <returns>EntryStruct</returns>
        public EntryStruct GetEntry(int col)
        {
            return entries[col];
        }

        /// <summary>
        /// Méthode permettant d'exécuter une action sur toutes les entrées d'un circuit
        /// </summary>
        /// <param name="action"></param>
        public void ForEachEntryStruct(Action<EntryStruct> action)
        {
            for (int j = 0; j < NbCol; j++)
                action(entries[j]);
        }

        /// <summary>
        /// Méthode permettant de fixer la valeur de l'entrée d'une colone
        /// </summary>
        /// <param name="col"></param>
        /// <param name="qubit"></param>
        public void SetEntry(int col, Qubit qubit)
        {
            EntryStruct entryStruct = entries[col];
            entryStruct.qubit = qubit;
            OnSetEntry?.Invoke(entryStruct);
        }

        /// <summary>
        /// Retourne l'état d'entrée du circuit.
        /// </summary>
        /// <returns>State</returns>
        public State GetEntryState()
        {
            return new State(FuncTools.Map((EntryStruct a) => a.qubit, entries));
        }

        /**
	     * ####################################################################################################
	     * ############################################### GATE ###############################################
	     * ####################################################################################################
	     */

        /// <summary>
        /// Crée un GateStruct et appelle l'évennement OnCreateGate.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="gate"></param>
        /// <returns>GateStruct</returns>
        private GateStruct CreateGateStruct(int row, int col, Gate gate)
        {
            GateStruct gateStruct = new GateStruct(row, col, gate);
            OnCreateGate?.Invoke(gateStruct);
            return gateStruct;
        }

        /// <summary>
        /// Retourne la GateStruct située aux row et col spécifiés.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>GateStruct</returns>
        public GateStruct GetGate(int row, int col)
        {
            return rows[row][col];
        }

        /// <summary>
        /// Méthode permettant d'exécuter une action sur toutes les portes d'un circuit.
        /// </summary>
        /// <param name="action"></param>
        public void ForEachGateStruct(Action<GateStruct> action)
        {
            for (int i = 0; i < NbRow; i++)
                for (int j = 0; j < NbCol; j += rows[i][j].gate.NbEntries)
                    action(rows[i][j]);
        }
        
        /// <summary>
        /// Retourne True si l'opération correspondante est garantie de réussir, False sinon (attention à l'atomicité de la transaction).
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="gate"></param>
        /// <returns>bool</returns>
        public bool CanPutGate(int row, int col, Gate gate)
        {
            // check for correct indexes
            if (row < 0 || row >= NbRow || col < 0 || col >= NbCol)
                throw new ArgumentOutOfRangeException("CanPutGate: row = " + row + " col = " + col);

            // check the gate would not overpass the circuit
            if (col + gate.NbEntries > NbCol)
                return false;

            // check for empty cases to put the gate
            for (int i = 0; i < gate.NbEntries; i++)
            {
                if (rows[row][col + i].gate != Gate.IDENTITY)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// Essaie de mettre la porte correspondante à la position spécifiée, renvoie True si réussi, False sinon.
        /// Une porte ne peut être posée uniquement si tous les emplacements nécessaires sont vides.
        /// Appelle l'évennement OnPutGate sur la nouvelle porte et OnRemoveGate sur les portes remplacées.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="gate"></param>
        /// <returns>bool</returns>
        public bool PutGate(int row, int col, Gate gate)
        {
            if (!CanPutGate(row, col, gate))
                return false;

            GateStruct newGateStruct = CreateGateStruct(row, col, gate);

            for (int i = 0; i < gate.NbEntries; i++)
            {
                GateStruct oldGateStruct = rows[row][col + i];
                rows[row][col + i] = newGateStruct;
                OnRemoveGate?.Invoke(oldGateStruct);
            }

            OnPutGate?.Invoke(newGateStruct);

            return true;
        }

        /// <summary>
        /// Retourne True si l'opération correspondante est garantie de réussir, False sinon (attention à l'atomicité de la transaction).
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>bool</returns>
        public bool CanRemoveGate(int row, int col)
        {
            if (row < 0 || row >= NbRow || col < 0 || col >= NbCol)
                throw new ArgumentOutOfRangeException("CanRemoveGate: row = " + row + " col = " + col);

            return rows[row][col].gate != Gate.IDENTITY;
        }
        /// <summary>
        /// Essaie de supprimer la porte correspondante à la position spécifiée, renvoie True si réussi, False sinon.
        /// Appelle l'évennement OnRemoveGate sur la porte ainsi supprimée.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>bool</returns>
        public bool RemoveGate(int row, int col)
        {
            if (!CanRemoveGate(row, col))
                return false;

            GateStruct gateStruct = rows[row][col];

            for (int i = 0; i < gateStruct.gate.NbEntries; i++)
            {
                GateStruct newGateStruct = CreateGateStruct(row, gateStruct.col + i, Gate.IDENTITY);
                rows[row][gateStruct.col + i] = newGateStruct;
                OnPutGate?.Invoke(newGateStruct);
            }

            OnRemoveGate?.Invoke(gateStruct);

            return true;
        }

        /// <summary>
        /// Retourne True si l'opération correspondante est garantie de réussir, False sinon (attention à l'atomicité de la transaction).
        /// </summary>
        /// <param name="sourceRow"></param>
        /// <param name="sourceCol"></param>
        /// <param name="targetRow"></param>
        /// <param name="targetCol"></param>
        /// <returns>bool</returns>
        public bool CanMoveGate(int sourceRow, int sourceCol, int targetRow, int targetCol)
        {
            if (sourceRow < 0 || sourceRow >= NbRow || targetRow < 0 || targetRow >= NbRow || sourceCol < 0 || sourceCol >= NbCol || targetCol < 0 || targetCol >= NbCol)
                throw new ArgumentOutOfRangeException("CanMoveGate: sourceRow = " + sourceRow + " " + targetRow + " sourceCol" + sourceCol + " targetCol = " + targetCol);

            if (sourceRow == targetRow && sourceCol == targetCol)
                return false;

            GateStruct gateStruct = rows[sourceRow][sourceCol];

            if (targetCol < 0 || targetCol + gateStruct.gate.NbEntries > NbCol)
                return false;

            for (int i = 0; i < gateStruct.gate.NbEntries; i++)
                if (rows[targetRow][targetCol + i].gate != Gate.IDENTITY && rows[targetRow][targetCol + i] != gateStruct)
                    return false;

            return true;
        }
        /// <summary>
        ///  Essaie de déplacer la porte de la position source à la position cible, renvoie True si réussie, False sinon.
        ///  Appelle l'évennement OnMoveGate sur les portes ainsi déplacées.
        /// </summary>
        /// <param name="sourceRow"></param>
        /// <param name="sourceCol"></param>
        /// <param name="targetRow"></param>
        /// <param name="targetCol"></param>
        /// <returns>bool</returns>
        public bool MoveGate(int sourceRow, int sourceCol, int targetRow, int targetCol)
        {
            if (!CanMoveGate(sourceRow, sourceCol, targetRow, targetCol))
                return false; ;
            
            GateStruct gateStruct = rows[sourceRow][sourceCol];

            sourceCol = gateStruct.col;

            gateStruct.row = targetRow;
            gateStruct.col = targetCol;

            for (int i = 0; i < gateStruct.gate.NbEntries; i++)
            {
                GateStruct movedGateStruct = rows[targetRow][targetCol + i];

                movedGateStruct.row = sourceRow;
                movedGateStruct.col = sourceCol + i;

                rows[sourceRow][sourceCol + i] = movedGateStruct;
                OnMoveGate?.Invoke(movedGateStruct);
                
                rows[targetRow][targetCol + i] = gateStruct;
            }

            OnMoveGate?.Invoke(gateStruct);

            return true;
        }

        /**
	     * ####################################################################################################
	     * ############################################### ROW ################################################
	     * ####################################################################################################
	     */
         
        /// <summary>
        /// Retourne la liste de portes correspondant à un ligne.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public List<GateStruct> GetRow(int rowIndex)
        {
            return FilterRow(rows[rowIndex]);
        }

        /// <summary>
        /// Retourne True si l'opération correspondante est garantie de réussir, False sinon (attention à l'atomicité de la transaction).
        /// </summary>
        /// <param name="index"></param>
        /// <returns>bool</returns>
        public bool CanInsertRow(int index)
        {
            // out of bound exception
            if (index < 0 || index > NbRow)
                throw new ArgumentOutOfRangeException("CanInsertRow: index = " + index);
            return true;
        }
        /// <summary>
        /// Essaie d'inserer un ligne vide à l'index spécifié, renvoie True si réussie, False sinon.
        /// Appelle l'évennement OnInsertRow sur la ligne ainsi insérée.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>bool</returns>
        public bool InsertRow(int index)
        {
            if (!CanInsertRow(index))
                return false;

            List<GateStruct> row = new List<GateStruct>(NbCol);
            for (int j = 0; j < NbCol; j++)
            {
                GateStruct gateStruct = CreateGateStruct(index, j, Gate.IDENTITY);
                row.Add(gateStruct);
            }
            rows.Insert(index, row);

            for (int i = index + 1; i < NbRow; i++)
            {
                List<GateStruct> updatedRow = rows[i];
                for (int j = 0; j < updatedRow.Count; j += updatedRow[j].gate.NbEntries)
                    updatedRow[j].row++;
            }

            OnInsertRow?.Invoke(row);

            return true;
        }

        /// <summary>
        /// Retourne True si l'opération correspondante est garantie de réussir, False sinon (attention à l'atomicité de la transaction).
        /// </summary>
        /// <param name="index"></param>
        /// <returns>bool</returns>
        public bool CanRemoveRow(int index)
        {
            // out of bound exception
            if (index < 0 || index >= NbRow)
                throw new ArgumentOutOfRangeException("CanRemoveRow: index = " + index);
            return true;
        }
        /// <summary>
        /// Essaie de supprimer la ligne d'index spécifié, renvoie True si réussie, False sinon.
        /// Appelle l'evennement OnRemoveRow sur la ligne ainsi supprimée.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>bool</returns>
        public bool RemoveRow(int index)
        {
            if (!CanRemoveRow(index))
                return false;

            List<GateStruct> removedRow = rows[index];
            List<GateStruct> filteredRemovedRow = new List<GateStruct>();
            for (int j = 0; j < removedRow.Count; j += removedRow[j].gate.NbEntries)
            {
                removedRow[j].row--;
                filteredRemovedRow.Add(removedRow[j]);
            }

            rows.RemoveAt(index);

            for (int i = index; i < NbRow; i++)
            {
                List<GateStruct> updatedRow = rows[i];
                for (int j = 0; j < updatedRow.Count; j += updatedRow[j].gate.NbEntries)
                    updatedRow[j].row--;
            }

            OnRemoveRow?.Invoke(filteredRemovedRow);

            return true;
        }

        /// <summary>
        /// Retourne True si l'opération correspondante est garantie de réussir, False sinon (attention à l'atomicité de la transaction).
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>bool</returns>
        public bool CanMoveRow(int source, int target)
        {
            // out of bound exception
            if (source < 0 || source >= NbRow || target < 0 || target >= NbRow)
                throw new ArgumentOutOfRangeException("CanRemoveRow: source = " + source + " target = " + target);

            if (source == target)
                return false;

            return true;
        }
        /// <summary>
        /// Essaie de déplacer la ligne d'index source à l'index cible, renvoie True si réussie, False sinon.
        /// Appelle l'évennement OnMoveRow sur chaque ligne dont l'index est mis à jour.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>bool</returns>
        public bool MoveRow(int source, int target)
        {
            // out of bound exception
            if (!CanMoveRow(source, target))
                return false;

            List<GateStruct> movedRow = rows[source];
            List<GateStruct> filteredMovedRow = new List<GateStruct>();
            rows.RemoveAt(source);
            for (int j = 0; j < movedRow.Count; j += movedRow[j].gate.NbEntries)
            {
                movedRow[j].row = target;
                filteredMovedRow.Add(movedRow[j]);
            }
            rows.Insert(target, movedRow);

            OnMoveRow?.Invoke(filteredMovedRow);

            // update the row property of moved rows
            if (source < target)
                for (int i = source; i < target; i++)
                {
                    List<GateStruct> updatedRow = rows[i];
                    List<GateStruct> filteredUpdatedRow = new List<GateStruct>();
                    for (int j = 0; j < updatedRow.Count; j += updatedRow[j].gate.NbEntries)
                    {
                        updatedRow[j].row--;
                        filteredUpdatedRow.Add(updatedRow[j]);
                    }
                    OnMoveRow?.Invoke(filteredUpdatedRow);
                }
            else
                for (int i = source; i > target; i--)
                {
                    List<GateStruct> updatedRow = rows[i];
                    List<GateStruct> filteredUpdatedRow = new List<GateStruct>();
                    for (int j = 0; j < updatedRow.Count; j += updatedRow[j].gate.NbEntries)
                    {
                        updatedRow[j].row++;
                        filteredUpdatedRow.Add(updatedRow[j]);
                    }
                    OnMoveRow?.Invoke(filteredUpdatedRow);
                }

            return true;
        }

        /**
	     * ####################################################################################################
	     * ############################################### COL ################################################
	     * ####################################################################################################
	     */

        /// <summary>
        /// Retourne True si l'opération correspondante est garantie de réussir, False sinon (attention à l'atomicité de la transaction).
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public List<GateStruct> GetCol(int columnIndex)
        {
            List<GateStruct> col = new List<GateStruct>(NbRow);
            for (int i = 0; i < NbRow; i++)
                if (rows[i][columnIndex].col == columnIndex)
                    col.Add(rows[i][columnIndex]);

            return col;
        }

        /// <summary>
        /// Retourne True si l'opération correspondante est garantie de réussir, False sinon (attention à l'atomicité de la transaction).
        /// </summary>
        /// <param name="index"></param>
        /// <returns>bool</returns>
        public bool CanInsertCol(int index)
        {
            // out of bound exception
            if (index < 0 || index > NbCol)
                throw new ArgumentOutOfRangeException("CanInsertCol: index = " + index);

            if (index > 0 && index < NbCol)
                // assure we can insert the column
                for (int i = 0; i < NbRow; i++)
                    if (rows[i][index].col < index)
                        return false;

            return true;
        }
        /// <summary>
        /// Essaie d'insérer une colone vide à l'index spécifié, renvoie True si réussie, False sinon.
        /// Associe à la colone insérée une entrée de valeur zéro.
        /// Echoue dans le cas ou la colone en question passerait au milieu d'une porte à plusieurs entrées.
        /// Appelle l'evennement OnInsertCol sur la colone ainsi insérée.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>bool</returns>
        public bool InsertCol(int index)
        {
            if (!CanInsertCol(index))
                return false;

            // insert the entry
            EntryStruct insertedEntry = CreateEntryStruct(index, Qubit.Zero);
            entries.Insert(index, insertedEntry);

            // insert the column
            List<GateStruct> insertedCol = new List<GateStruct>(NbRow);
            for (int i = 0; i < NbRow; i++)
            {
                GateStruct gateStruct = CreateGateStruct(i, index, Gate.IDENTITY);
                insertedCol.Add(gateStruct);
                rows[i].Insert(index, gateStruct);
            }

            for (int j = NbCol - 1; j >= index + 1; j--)
            {
                entries[j].col++;
                for (int i = 0; i < NbRow; i++)
                    if (rows[i][j].col == j - 1)
                        rows[i][j].col++;
            }

            OnInsertCol?.Invoke(insertedEntry, insertedCol);

            return true;
        }
        
        /// <summary>
        /// Retourne True si l'opération correspondante est garantie de réussir, False sinon (attention à l'atomicité de la transaction).
        /// </summary>
        /// <param name="index"></param>
        /// <returns>bool</returns>
        public bool CanRemoveCol(int index)
        {
            // out of bound exception
            if (index < 0 || index >= NbCol)
                throw new ArgumentOutOfRangeException("CanRemoveCol: index = " + index);

            // assure we can remove the column
            for (int i = 0; i < NbRow; i++)
                if (rows[i][index].gate.NbEntries > 1)
                    return false;

            return true;
        }
        /// <summary>
        /// Essaie de supprime la colone d'index spécifié, renvoie True si réussie, False sinon.
        /// Echoue dans le cas ou une porte à plusieurs entrées occupe la colone en question.
        /// Appelle l'évennement OnRemoveCol sur la colone ainsi supprimée.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>bool</returns>
        public bool RemoveCol(int index)
        {
            if (!CanRemoveCol(index))
                return false;

            EntryStruct removedEntry = entries[index];
            entries.RemoveAt(index);

            List<GateStruct> removedCol = new List<GateStruct>();
            for (int i = 0; i < NbRow; i++)
            {
                GateStruct gateStruct = rows[i][index];
                removedCol.Add(gateStruct);
                rows[i].RemoveAt(index);
            }

            for (int j = index; j < NbCol; j++)
            {
                entries[j].col--;

                for (int i = 0; i < NbRow; i++)
                    if (rows[i][j].col == j + 1)
                        rows[i][j].col--;
            }

            OnRemoveCol?.Invoke(removedEntry, removedCol);

            return true;
        }

        /// <summary>
        /// Retourne True si l'opération correspondante est garantie de réussir, False sinon (attention à l'atomicité de la transaction).
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>bool</returns>
        public bool CanMoveCol(int source, int target)
        {
            if (source < 0 || source >= NbCol || target < 0 || target >= NbCol)
                throw new ArgumentOutOfRangeException("CanMoveCol: source = " + source + " target = " + target);

            if (source == target)
                return false;

            // assure we can move the source column
            for (int i = 0; i < NbRow; i++)
                if (rows[i][source].gate.NbEntries > 1)
                    return false;

            if (source < target)
                target++;

            // assure we can insert the source column
            if (target > 0 && target < NbCol)
                for (int i = 0; i < NbRow; i++)
                    if (rows[i][target].col < target)
                        return false;

            return true;
        }
        /// <summary>
        /// Essaie de déplacer la colone source à l'index cible, renvoie True si réussie, False sinon.
        /// Echoue dans le cas ou une porte à plusieurs entrées occupe la colone source ou que la colone en question passerait au milieu d'une porte à plusieurs entrées à l'index cible.
        /// Appelle l'évennement OnMoveCol sur chaque colone dont l'index est mis à jour.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool MoveCol(int source, int target)
        {
            if (!CanMoveCol(source, target))
                return false;

            // move the entry
            EntryStruct movedEntry = entries[source];
            entries.RemoveAt(source);
            movedEntry.col = target;
            entries.Insert(target, movedEntry);

            // move the column
            List<GateStruct> movedCol = new List<GateStruct>(NbRow);
            for (int i = 0; i < NbRow; i++)
            {
                List<GateStruct> currentRow = rows[i];
                GateStruct gateStruct = currentRow[source];
                currentRow.RemoveAt(source);
                gateStruct.col = target;
                currentRow.Insert(target, gateStruct);
                movedCol.Add(gateStruct);
            }

            OnMoveCol?.Invoke(movedEntry, movedCol);

            // update col property of moved gates
            if (source < target)
                for (int j = source; j < target; j++)
                {
                    EntryStruct updatedEntry = entries[j];
                    updatedEntry.col--;

                    List<GateStruct> updatedCol = new List<GateStruct>(NbRow);
                    for (int i = 0; i < NbRow; i++)
                        if (rows[i][j].col == j + 1)
                        {
                            rows[i][j].col--;
                            updatedCol.Add(rows[i][j]);
                        }
                    OnMoveCol?.Invoke(updatedEntry, updatedCol);
                }
            else
                for (int j = source; j > target; j--)
                {
                    EntryStruct updatedEntry = entries[j];
                    updatedEntry.col++;

                    List<GateStruct> updatedCol = new List<GateStruct>(NbRow);
                    for (int i = 0; i < NbRow; i++)
                        if (rows[i][j].col == j - 1)
                        {
                            rows[i][j].col++;
                            updatedCol.Add(rows[i][j]);
                        }
                    OnMoveCol?.Invoke(updatedEntry, updatedCol);
                }

            return true;
        }

        /**
	     * ####################################################################################################
	     * ########################################### EVALUATION #############################################
	     * ####################################################################################################
	     */

        /// <summary>
        /// Transforme un ligne du tableau représqentrant le circuit en liste de portes 
        /// </summary>
        /// <param name="rawRow"></param>
        /// <returns></returns>
        private List<GateStruct> FilterRow(List<GateStruct> rawRow)
        {
            List<GateStruct> filteredRow = new List<GateStruct>();
            for (int j = 0; j < rawRow.Count; j += rawRow[j].gate.NbEntries)
                filteredRow.Add(rawRow[j]);
            return filteredRow;
        }

        /// <summary>
        /// Retourne True si toutes les portes de la ligne spécifiée sont des portes IDENTITY, False sinon.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>bool</returns>
        public bool IsRowEmpty(int index)
        {
            return FuncTools.AllEquals(Gate.IDENTITY, FuncTools.Map((GateStruct a) => a.gate, rows[index]));
        }

        /// <summary>
        /// Convertit une ligne en une porte (produit tensoriel des toutes les portes de la ligne).
        /// </summary>
        /// <param name="row"></param>
        /// <returns>Gate</returns>
        public Gate GetRowGate(List<GateStruct> row)
        {
            return Gate.Kron(FuncTools.Map((GateStruct a) => a.gate, FilterRow(row)));
        }

        /// <summary>
        /// Convertit le circuit en porte (produit matriciel des portes représenttant les lignes).
        /// </summary>
        /// <returns>Gate</returns>
        public Gate GetCircuitGate()
        {
            return Gate.Add(FuncTools.Map(GetRowGate, rows));
        }

        /// <summary>
        /// Convertit le circuit en porte (produit matriciel des portes représenttant les lignes)
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Gate</returns>
        public Gate GetCircuitGate(string name)
        {
            return new Gate(name, Gate.Add(FuncTools.Map(GetRowGate, rows)));
        }

        /// <summary>
        /// Evalue partiellement le circuit jusquà la ligne de numéro spécifié
        /// </summary>
        /// <param name="till_row"></param>
        /// <returns>State</returns>
        public State Evaluate(int till_row)
        {
            State entry = GetEntryState();
            return new State(LinearAlgebra.Mult(entry.Vector, Gate.Add(FuncTools.Map(GetRowGate, FuncTools.Take(till_row + 1, rows))).Matrix));
        }
        /// <summary>
        /// Evalue le circuit.
        /// </summary>
        /// <param name="till_row"></param>
        /// <returns>State</returns>
        public State Evaluate()
        {
            return Evaluate(NbRow - 1);
        }

        /// <summary>
        /// Convertit un circuit en chaine de caratères le représentant
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            string s = "entry:\n" + GetEntryState();

            for (int i = 0; i < NbRow; i++)
            {
                if (IsRowEmpty(i))
                    continue;

                s += "\nrow " + i + ": " + rows[i];
            }

            return s;
        }
    }
}
