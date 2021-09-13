using System.Collections.Generic;


namespace WpfApp1
{

    public class SymbolList
    {
        private List<string> _symbols;
        private string _symbol { get; set; }
        public SymbolList()
        {
            _symbols = new List<string>();
        }
        public void AddSymbol(string symbol)
        {
            this._symbol = symbol;
            _symbols.Add(_symbol);
        }
    }

}
