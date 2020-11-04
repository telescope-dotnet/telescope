using System;
using Sharp7;
using TeleScope.Connectors.Abstractions;
using TeleScope.Connectors.Abstractions.Events;
using TeleScope.Connectors.Plc.Abstractions;
using TeleScope.Connectors.Plc.Siemens.Extensions;
using TeleScope.Connectors.Plc.Siemens.Events;


namespace TeleScope.Connectors.Plc.Siemens
{
	public class S7Connector : IPlcConnectable
	{
		// -- fields

		private S7Client _client;
		private S7Setup _setup;

		private S7Selector _parameter;

		// -- events

		public event ConnectorEventHandler Connected;
		public event ConnectorEventHandler Disconnected;
		public event ConnectorFailedEventHandler Error;

		// -- properties

		/// <summary>
		/// Gets the information if the client is instanciated and connected or not.
		/// </summary>
		public bool IsConnected => (_client != null ? _client.Connected : false);

		/// <summary>
		/// Gets the last received result code from the connected PLC.
		/// Attention: Async or concurrent calls may override the value.
		/// </summary>
		public int ResultCode { get; private set; }

		/// <summary>
		/// Gets the last received result as a string representation from the connected PLC.
		/// Attention: Async or concurrent calls may override the value.
		/// </summary>
		public string Result => S7Results.GetString(ResultCode);

		// -- constructors

		/// <summary>
		/// The default empty constructor instanciates the internal S7 client.
		/// </summary>
		public S7Connector()
		{
			_client = new S7Client();
		}

		// -- methods

		/// <summary>
		/// Takes the setup instance and casts it into the specific type of Sharp7Setup
		/// </summary>
		/// <param name="setup">The concrete setup for the connector.</param>
		/// <returns>The calling instance.</returns>
		public IConnectable Setup(SetupBase s7Setup)
		{
			_setup = s7Setup as S7Setup;
			return this;
		}

		/// <summary>
		/// Opens the connection to the PLC.
		/// </summary>
		/// <returns>The calling instance.</returns>
		public IConnectable Connect()
		{
			// TODO: use a proper initial result code
			var result = -1;

			try
			{
				result = _client.ConnectTo(_setup.IPAddress, _setup.Rack, _setup.Slot);
				ResultCode = result;
				Connected?.Invoke(this,
					new SiemensConnectorEventArgs(_setup.Name, result, S7Results.GetString(result)));
			}
			catch (Exception ex)
			{
				Error?.Invoke(this,
					new SiemensConnectorFailedEventArgs(ex, result, S7Results.GetString(result), _setup.Name));
			}

			return this;
		}

		/// <summary>
		/// Closes the connection from the PLC. 
		/// </summary>
		/// <returns>The calling instance.</returns>
		public IConnectable Disconnect()
		{
			var result = _client.Disconnect();
			ResultCode = result;
			Disconnected?.Invoke(this,
				new SiemensConnectorEventArgs(_setup.Name, result, S7Results.GetString(result)));
			return this;
		}

		/// <summary>
		/// Stores the select parameters to read or write to the PLC lateron. 
		/// </summary>
		/// <param name="parameter">The S7 specific parameters.</param>
		/// <returns>The calling instance.</returns>
		public IPlcConnectable Select(S7Selector parameter)
		{
			_parameter = parameter;
			return this;
		}

		/// <summary>
		/// Stores the select parameters in the internal structure Sharp7Parameter and validates the input.
		/// Supported types are S7Selector, strings with proper structure and integer arrays, representing
		/// the datablock [0], the bit offset [1] and count [2] for bit offsets or character counts. 
		/// </summary>
		/// <param name="parameter">The generic object may contain the select parameters in different forms.</param>
		/// <returns>The calling instance.</returns>
		public IPlcConnectable Select(object parameter)
		{
			try
			{
				if (parameter.GetType().IsAssignableFrom(typeof(S7Selector)))
				{
					return Select((S7Selector)parameter);
				}
				else if (parameter.GetType().IsAssignableFrom(typeof(string)))
				{
					/*
					 * Examples: 
					 * "DB652.DBD82"
					 * "DB652.DBX82.1"  // last values` is the number of the bit, range [0..7]
					 * 
					 * Meanings:
					 * DBD = Double
					 * DBW = Word
					 * DBB = Byte
					 * DBX = Bit
					 */
					var splits = (parameter as string).Split('.');
					var datablock = int.Parse(splits[0].Remove(0, 2));
					var offset = int.Parse(splits[1].Remove(0, 3));
					var number = (splits.Length == 3 ? int.Parse(splits[3]) : 0);

					return Select(new S7Selector(datablock, offset, number));
				}
				else if (parameter.GetType().IsArray)
				{
					var input = (Array)parameter;
					object[] parameters = new object[input.Length];
					for (int i = 0; i < input.Length; i++)
					{
						parameters[i] = input.GetValue(i);
					}
					return Select(parameters);
				}
				else
				{
					throw new ArgumentException(
						$"The parameter could not be adapted into the internal representation of '{typeof(S7Selector)}'.");
				}
			}
			catch (Exception ex)
			{
				fowardError(ex);
			}

			return this;

			// -- local function

			void fowardError(Exception ex)
			{
				var result = S7Results.CliInvalidParams;
				Error?.Invoke(this,
					   new SiemensConnectorFailedEventArgs(ex, result, S7Results.GetString(result), _setup.Name));
			}
		}

		/// <summary>
		/// Stores the select parameters in the internal structure Sharp7Parameter and validates the array beforehand.
		/// The parameter array must contain three integers repesenting the datablock [0], the bit offset [1] and
		/// the count [2] for bit offsets or character counts.
		/// </summary>
		/// <param name="parameters">The array of S7 parameters to select and read lateron the data.</param>
		/// <returns>The calling instance.</returns>
		public IPlcConnectable Select(object[] parameters)
		{
			var length = 3;
			int result = 0;
			try
			{
				validateLength();
				validate(0, typeof(int));
				validate(1, typeof(int));
				validate(2, typeof(int));
			}
			catch (Exception ex)
			{
				Error?.Invoke(this,
				   new SiemensConnectorFailedEventArgs(ex, result, S7Results.GetString(result), _setup.Name));

				return this;
			}

			return Select(new S7Selector((int)parameters[0], (int)parameters[1], (int)parameters[2]));

			// -- local functions

			void validateLength()
			{
				if (parameters.Length != length)
				{
					result = S7Results.CliInvalidParamNumber;
					throw new ArgumentException($"The array of parameters should contain {length} elements but has {parameters.Length} elements.");
				}
			}

			void validate(int index, Type type)
			{
				if (!parameters[index].GetType().IsAssignableFrom(type))
				{
					result = S7Results.CliInvalidParams;
					throw new ArgumentException($"The parameter[{index}] should be of type '{type}' but is assigned as '{parameters[0].GetType()}'.");
				}
			}
		}

		/// <summary>
		/// A generic read method that covers most common types.
		/// </summary>
		/// <typeparam name="T">The return type of the method.</typeparam>
		/// <returns>The result value of type T.</returns>
		public T Read<T>()
		{
			T obj = default(T);
			Type type = typeof(T);
			int size;
			byte[] buffer;
			int result = 0;

			try
			{
				if (type.IsAssignableFrom(typeof(bool)))
				{
					size = sizeof(bool).ToBits();
					read();
					obj = (T)Convert.ChangeType(S7.GetBitAt(buffer, 0, _parameter.Number), type);
				}
				else if (type.IsAssignableFrom(typeof(byte)))
				{
					size = sizeof(byte).ToBits();
					read();
					obj = (T)Convert.ChangeType(S7.GetByteAt(buffer, 0), type);
				}
				else if (type.IsAssignableFrom(typeof(sbyte)))
				{
					size = sizeof(sbyte).ToBits();
					read();
					obj = (T)Convert.ChangeType(S7.GetByteAt(buffer, 0), type);
				}
				else if (type.IsAssignableFrom(typeof(short)))
				{
					size = sizeof(short).ToBits();
					read();
					obj = (T)Convert.ChangeType(S7.GetSIntAt(buffer, 0), type);
				}
				else if (type.IsAssignableFrom(typeof(ushort)))
				{
					size = sizeof(ushort).ToBits();
					read();
					obj = (T)Convert.ChangeType(S7.GetUIntAt(buffer, 0), type);
				}
				else if (type.IsAssignableFrom(typeof(int)))
				{
					size = sizeof(int).ToBits();
					read();
					obj = (T)Convert.ChangeType(S7.GetIntAt(buffer, 0), type);
				}
				else if (type.IsAssignableFrom(typeof(float)))
				{
					size = sizeof(float).ToBits();
					read();
					obj = (T)Convert.ChangeType(S7.GetRealAt(buffer, 0), type);
				}
				else if (type.IsAssignableFrom(typeof(double)))
				{
					size = sizeof(double).ToBits();
					read();
					obj = (T)Convert.ChangeType(S7.GetLRealAt(buffer, 0), type);
				}
				else if (type.IsAssignableFrom(typeof(long)))
				{
					size = sizeof(long).ToBits();
					read();
					obj = (T)Convert.ChangeType(S7.GetLIntAt(buffer, 0), type);
				}
				else if (type.IsAssignableFrom(typeof(ulong)))
				{
					size = sizeof(ulong).ToBits();
					read();
					obj = (T)Convert.ChangeType(S7.GetULIntAt(buffer, 0), type);
				}
				else if (type.IsAssignableFrom(typeof(DateTime)))
				{
					size = 8.ToBits();
					read();
					obj = (T)Convert.ChangeType(S7.GetDateTimeAt(buffer, 0), type);
				}
				else if (type.IsAssignableFrom(typeof(char)))
				{
					size = sizeof(char).ToBits();
					read();
					obj = (T)Convert.ChangeType(S7.GetWordAt(buffer, 0), type);
				}
				else if (type.IsAssignableFrom(typeof(string)))
				{
					size = (sizeof(char) * _parameter.Number).ToBits();
					read();
					obj = (T)Convert.ChangeType(S7.GetStringAt(buffer, 0), type);
				}
			}
			catch (Exception ex)
			{
				Error?.Invoke(this,
					new SiemensConnectorFailedEventArgs(ex, result, S7Results.GetString(result), _setup.Name));
			}

			return obj;

			// -- local function

			void read()
			{
				buffer = new byte[size];
				result = _client.DBRead(_parameter.Datablock, _parameter.BitOffset, size, buffer);
				ResultCode = result;
			}
		}

		public void Write<T>(T data)
		{
			throw new NotImplementedException();
		}
	}
}
